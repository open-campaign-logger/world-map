﻿/// <binding BeforeBuild='default' />
// Copyright 2017-2019 Jochen Linnemann, Cory Gill
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

/// <binding BeforeBuild='beforeBuild' />
module.exports = function(grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        copy: {
            leaflet: {
                files: [
                    {
                        expand: true,
                        cwd: 'node_modules/leaflet/dist',
                        src: 'leaflet.js',
                        dest: 'wwwroot/js/'
                    },
                    {
                        expand: true,
                        cwd: 'node_modules/leaflet/dist',
                        src: 'leaflet.css',
                        dest: 'wwwroot/css/'
                    },
                    {
                        expand: true,
                        cwd: 'node_modules/leaflet/dist/images',
                        src: '**',
                        dest: 'wwwroot/css/images/'
                    }
                ]
            },
            leaflet_draw: {
                files: [
                    {
                        expand: true,
                        cwd: 'node_modules/leaflet-draw/dist',
                        src: 'leaflet.draw.js',
                        dest: 'wwwroot/js/'
                    },
                    {
                        expand: true,
                        cwd: 'node_modules/leaflet-draw/dist',
                        src: 'leaflet.draw.css',
                        dest: 'wwwroot/css/'
                    },
                    {
                        expand: true,
                        cwd: 'node_modules/leaflet-draw/dist/images',
                        src: '**',
                        dest: 'wwwroot/css/images/'
                    }
                ]
            },
            quill: {
                files: [
                    {
                        expand: true,
                        cwd: 'node_modules/quill/dist',
                        src: 'quill.js',
                        dest: 'wwwroot/js/'
                    },
                    {
                        expand: true,
                        cwd: 'node_modules/quill/dist',
                        src: '*.css',
                        dest: 'wwwroot/css/'
                    }
                ]
            },
            oidcclient: {
                files: [
                    {
                        expand: true,
                        cwd: 'node_modules/oidc-client/dist',
                        src: 'oidc-client.js',
                        dest: 'wwwroot/js/'
                    }
                ]
            },
            jQuery: {
                files: [
                    {
                        expand: true,
                        cwd: 'node_modules/jQuery/dist',
                        src: '**',
                        dest: 'wwwroot/js/'
                    },
                    {
                        expand: true,
                        cwd: 'node_modules/jquery-validation/dist',
                        src: '*.js',
                        dest: 'wwwroot/js/'
                    },
                    {
                        expand: true,
                        cwd: 'node_modules/jquery-validation-unobtrusive',
                        src: 'jquery.validate.unobtrusive.js',
                        dest: 'wwwroot/js/'
                    }
                ]
            },
            bootstrap: {
                files: [
                    {
                        expand: true,
                        cwd: 'node_modules/bootstrap-sass/assets/fonts',
                        src: '**',
                        dest: 'wwwroot/fonts/'
                    },
                    {
                        expand: true,
                        cwd: 'node_modules/bootstrap-sass/assets/javascripts',
                        src: ['bootstrap.js', 'bootstrap.min.js'],
                        dest: 'wwwroot/js/'
                    },
                    {
                        expand: true,
                        cwd: 'node_modules/bootstrap-dialog/dist/css',
                        src: ['bootstrap-dialog.css', 'bootstrap-dialog.min.css'],
                        dest: 'wwwroot/css/'
                    },
                    {
                        expand: true,
                        cwd: 'node_modules/bootstrap-dialog/dist/js',
                        src: ['bootstrap-dialog.js', 'bootstrap-dialog.min.js'],
                        dest: 'wwwroot/js/'
                    }
                ]
            }
        },
        sass: {
            "wwwroot/css/worldmap.css": ['wwwroot/css/worldmap.scss']
        },
        subgrunt: {
            bootswatch: {
                options: {
                    passGruntFlags: false
                },
                projects: {
                    'node_modules/bootswatch-sass': 'default'
                }
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-sass');
    grunt.loadNpmTasks('grunt-subgrunt');

    grunt.registerTask('default',
        ['copy:leaflet', 'copy:jQuery', 'copy:bootstrap', 'copy:leaflet_draw', 'copy:quill', 'copy:oidcclient']);
    grunt.registerTask('default-sass', ['default', 'sass']);
    grunt.registerTask('full', ['subgrunt:bootswatch', 'default']);
    grunt.registerTask('beforeBuild', ['default']);
};