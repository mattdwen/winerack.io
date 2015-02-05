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
        files: ['Content/_scss/{,*/}*.{scss,sass}'],
        tasks: ['sass:serve', 'autoprefixer']
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
          cwd: 'Content/_scss',
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
      serve: {
        src: ['bower_components/bootstrap-sass-official/assets/javascripts/bootstrap.js'],
        dest: 'Scripts/dist/vendor.js'
      }
    }
  });

  // Tasks
  // ========================================================================

  grunt.registerTask('serve', [
    'sass:serve',
    'concat:serve',
    'autoprefixer',
    'watch'
  ]);

  grunt.registerTask('default', [
    'serve'
  ]);

};
