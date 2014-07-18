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

        // Compass & Sass
        // --------------------------------------------------------------------
        compass: {
            options: {
                sassDir: 'Content/_scss',
                cssDir: 'Content/css',
                importPath: 'bower_components'
            },
            serve: {
                options: {
                    debugInfo: true
                }
            }
        },

        // Concatenate Javascript files
        // --------------------------------------------------------------------
        concat: {
            serve: {
                src: [
                    'bower_components/bootstrap-sass-official/assets/javascripts/bootstrap.js'
                ],
                dest: 'Scripts/dist/vendor.js'
            }
        }
    });

    // Tasks
    // ========================================================================

    grunt.registerTask('serve', [
        'compass:serve',
        'concat:serve'
    ]);

    grunt.registerTask('default', [
        'serve'
    ]);

};