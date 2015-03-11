using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace winerack.Logic {
	public static class Images {

		#region Private Methods

		private static ImageCodecInfo GetEncoder(ImageFormat format) {
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			foreach (var codec in codecs) {
				if (codec.FormatID == format.Guid) {
					return codec;
				}
			}
			return null;
		}

		private static EncoderParameters GetEncoderParameters() {
			var encoder = Encoder.Quality;
			var encoderParameters = new EncoderParameters(1);
			var encoderParameter = new EncoderParameter(encoder, 80L);
			encoderParameters.Param[0] = encoderParameter;
			return encoderParameters;
		}

		#endregion Private Methods

		#region Public Methods

		/// <summary>Resizes an image to a new width and height.</summary>
		/// <param name="Stream">Data stream of the image
		/// <param name="maximumWidth">When resizing the image, this is the maximum width to resize the image to.</param>
		/// <param name="maximumHeight">When resizing the image, this is the maximum height to resize the image to.</param>
		/// <param name="enforceRatio">Indicates whether to keep the width/height ratio aspect or not. If set to false, images with an unequal width and height will be distorted and padding is disregarded. If set to true, the width/height ratio aspect is maintained and distortion does not occur.</param>
		/// <param name="addPadding">Indicates whether fill the smaller dimension of the image with a white background. If set to true, the white padding fills the smaller dimension until it reach the specified max width or height. This is used for maintaining a 1:1 ratio if the max width and height are the same.</param>
		/// <returns>Stream to upload</returns>
		public static MemoryStream Resize(Stream source, int maximumWidth, int maximumHeight, bool enforceRatio, bool addPadding) {
			var image = Image.FromStream(source);
			var imageEncoders = ImageCodecInfo.GetImageEncoders();
			var canvasWidth = maximumWidth;
			var canvasHeight = maximumHeight;
			var newImageWidth = maximumWidth;
			var newImageHeight = maximumHeight;
			var xPosition = 0;
			var yPosition = 0;

			if (enforceRatio) {
				var ratioX = maximumWidth / (double)image.Width;
				var ratioY = maximumHeight / (double)image.Height;
				var ratio = ratioX < ratioY ? ratioX : ratioY;
				newImageHeight = (int)(image.Height * ratio);
				newImageWidth = (int)(image.Width * ratio);

				if (addPadding) {
					xPosition = (int)((maximumWidth - (image.Width * ratio)) / 2);
					yPosition = (int)((maximumHeight - (image.Height * ratio)) / 2);
				} else {
					canvasWidth = newImageWidth;
					canvasHeight = newImageHeight;
				}
			}

			var thumbnail = new Bitmap(canvasWidth, canvasHeight);
			var graphic = Graphics.FromImage(thumbnail);

			if (enforceRatio && addPadding) {
				graphic.Clear(Color.White);
			}

			graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphic.SmoothingMode = SmoothingMode.HighQuality;
			graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphic.CompositingQuality = CompositingQuality.HighQuality;
			graphic.DrawImage(image, xPosition, yPosition, newImageWidth, newImageHeight);

			var output = new MemoryStream();
			var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
			var encoderParameters = GetEncoderParameters();
			thumbnail.Save(output, jpgEncoder, encoderParameters);
			output.Position = 0;

			return output;
		}

		public static MemoryStream ResizeAndCrop(Stream source, int Width, int Height, bool needToFill) {
			var image = Image.FromStream(source);
			int sourceWidth = image.Width;
			int sourceHeight = image.Height;
			int sourceX = 0;
			int sourceY = 0;
			double destX = 0;
			double destY = 0;

			double nScale = 0;
			double nScaleW = 0;
			double nScaleH = 0;

			nScaleW = ((double)Width / (double)sourceWidth);
			nScaleH = ((double)Height / (double)sourceHeight);
			if (!needToFill) {
				nScale = Math.Min(nScaleH, nScaleW);
			} else {
				nScale = Math.Max(nScaleH, nScaleW);
				destY = (Height - sourceHeight * nScale) / 2;
				destX = (Width - sourceWidth * nScale) / 2;
			}

			if (nScale > 1) {
				nScale = 1;
			}

			int destWidth = (int)Math.Round(sourceWidth * nScale);
			int destHeight = (int)Math.Round(sourceHeight * nScale);

			System.Drawing.Bitmap bmPhoto = null;

			try {
				bmPhoto = new System.Drawing.Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
			} catch (Exception ex) {
				throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
					destWidth, destX, destHeight, destY, Width, Height), ex);
			}

			using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto)) {
				var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
				var encoderParameters = GetEncoderParameters();

				grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
				grPhoto.CompositingQuality = CompositingQuality.HighQuality;
				grPhoto.SmoothingMode = SmoothingMode.HighQuality;

				Rectangle to = new System.Drawing.Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
				Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
				grPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);

				var output = new MemoryStream();
				bmPhoto.Save(output, jpgEncoder, encoderParameters);
				output.Position = 0;

				return output;
			}
		}

		public static IList<ImageSize> GetSizes(ImageSizeSets set) {
			var result = new List<ImageSize>();

			// Two square thumbnails for all
			result.Add(new ImageSize { Suffix = "sq", MaxWidth = 150, MaxHeight = 150, Crop = true });
			result.Add(new ImageSize { Suffix = "sq_sm", MaxWidth = 75, MaxHeight = 75, Crop = true });

			switch (set) {
				case ImageSizeSets.Profile:
					result.Add(new ImageSize { Suffix = "sq_md", MaxWidth = 250, MaxHeight = 250, Crop = true });
					break;

				case ImageSizeSets.Standard:
					result.Add(new ImageSize { Suffix = "sm", MaxWidth = 320, MaxHeight = 320, Crop = false });
					result.Add(new ImageSize { Suffix = "md", MaxWidth = 640, MaxHeight = 640, Crop = false });
					result.Add(new ImageSize { Suffix = "lg", MaxWidth = 1024, MaxHeight = 1024, Crop = false });
					break;

				default:
					throw new NotImplementedException();
			}

			return result;
		}

		#endregion Public Methods

	}

	public class ImageSize {

		#region Properties

		public string Suffix { get; set; }

		public int MaxWidth { get; set; }

		public int MaxHeight { get; set; }

		public bool Crop { get; set; }

		#endregion Properties

	}

	public enum ImageSizeSets {
		Profile,
		Standard
	}

}