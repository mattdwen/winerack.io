module.exports = function (grunt) {
  // Setup
  // ========================================================================

  // Show elapsed time after tasks run
  require('time-grunt')(grunt);

  // Load all Grunt tasks
  require('load-grunt-tasks')(grunt);

  // Config
  // ========================================================================

  grunt.initConfig({
    // Watches files for changes and runs tasks based on the changed files
    watch: {
      sass: {
        files: ['Content/sass/{,*/}*.{scss,sass}'],
        tasks: ['sass:serve', 'autoprefixer']
      }
    },

    // Clean up old files
    clean: {
      server: [
        'Content/fonts/font-awesome'
      ]
    },

    // Copy files
    copy: {
      fonts: {
        expand: true,
        cwd: 'bower_components/font-awesome/fonts',
        src: '*',
        dest: 'Content/fonts/font-awesome'
      },
      select2: {
        expand: true,
        cwd: 'bower_components/select2',
        src: '*.{css,png,gif}',
        dest: 'Content/vendor/select2'
      }
    },

    // Compiles Sass to CSS and generates necessary files if requested
    sass: {
      options: {
        loadPath: 'bower_components'
      },
      serve: {
        files: [{
          expand: true,
          cwd: 'Content/sass',
          src: ['*.{scss,sass}'],
          dest: 'Content/css',
          ext: '.css'
        }]
      }
    },

    // Add vendor prefixed styles
    autoprefixer: {
      options: {
        browsers: ['> 1%', 'last 2 versions', 'Firefox ESR', 'Opera 12.1']
      },
      dist: {
        files: [{
          expand: true,
          cwd: 'Content/styles/',
          src: 'main.css',
          dest: 'Content/styles/'
        }]
      }
    },

    // Concatenate Javascript files
    concat: {
      vendor: {
        src: [
          'bower_components/bootstrap-sass-official/assets/javascripts/bootstrap.js',
          'bower_components/imagesloaded/imagesloaded.pkgd.js',
          'bower_components/masonry/dist/masonry.pkgd.js',
          'bower_components/select2/select2.js',
          'bower_components/typeahead.js/dist/bloodhound.js',
          'bower_components/typeahead.js/dist/typeahead.jquery.js'
        ],
        dest: 'Scripts/dist/vendor.js'
      },
      angular: {
        src: [
          'bower_components/angular/angular.js',
          'bower_components/angular-resource/angular-resource.js',
          'bower_components/angular-route/angular-route.js',
          'bower_components/angular-sanitize/angular-sanitize.js',
          'bower_components/angular-ui-select2/src/select2.js',
          'bower_components/angular-ui-utils/ui-utils.js',
        ],
        dest: 'Scripts/dist/angular.js'
      }
    },

    // Tasks to run at the same time
    concurrent: {
      server: ['sass:serve', 'copy']
    }
});

// Tasks
// ========================================================================

grunt.registerTask('serve', [
  'clean:server',
  'concurrent:server',
  'concat',
  'autoprefixer',
  'watch'
]);

grunt.registerTask('default', [
  'serve'
]);
};