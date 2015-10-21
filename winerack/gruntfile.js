module.exports = function (grunt) {

  // Configuration
  // --------------------------------------------------------------------------

  grunt.initConfig({

    // Clean
    //
    clean: {
      lib: ['./wwwroot/lib/']
    },

    // Copy files
    //
    copy: {
      bower: {
        expand: true,
        cwd: './bower_components/',
        src: [
          'bootstrap/dist/js/bootstrap.*',
          'jquery/dist/jquery.*',
          'jquery-validation/dist/jquery.validate*',
          'jquery-validation-unobtrusive/*.js'
        ],
        dest: './wwwroot/lib/'
      },
      fonts: {
        expand: true,
        cwd: './bower_components/font-awesome/fonts',
        src: '*',
        dest: './wwwroot/lib/font-awesome'
      },
    },
    
    // Compile Sass stylesheets
    //
    sass: {
      options: {
        loadPath: 'bower_components'
      },
      main: {
        files: [{
          expand: true,
          cwd: './Styles',
          src: '*.{scss,sass}',
          dest: './wwwroot/css',
          ext: '.css'
        }]
      }
    },

    // Watch for changes
    //
    watch: {
      sass: {
        files: './Styles/**/*.{scss,sass}',
        tasks: ['sass']
      }
    }
    
  });

  // Load tasks
  // --------------------------------------------------------------------------

  require('load-grunt-tasks')(grunt);



  // Tasks
  // --------------------------------------------------------------------------

  grunt.registerTask('build', [
    'clean',
    'copy',
    'sass'
  ]);

  grunt.registerTask('dev', [
    'build',
    'watch'
  ]);
};