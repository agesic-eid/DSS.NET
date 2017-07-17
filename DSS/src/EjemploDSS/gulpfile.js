/// <binding BeforeBuild='clean' AfterBuild='min, copy' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var paths = {
    webroot: "./wwwroot/",
    packages: "./bower_packages/"
};

paths.jsSources = [
    "./Scripts/**/*.js",
    paths.packages + "jquery/dist/jquery.js",
    paths.packages + "bootstrap/dist/js/bootstrap.js"
];

paths.cssSources = [
    "./Styles/**/*.css",
    paths.packages + "bootstrap/dist/css/bootstrap.css"
];

paths.fontsSources = [
    paths.packages + "bootstrap/dist/fonts/*.*"
];

paths.jsDest = paths.webroot + "js/";
paths.cssDest = paths.webroot + "css/";
paths.fontsDest = paths.webroot + "fonts/";

paths.jsMinDest = paths.jsDest + "scripts.min.js";
paths.cssMinDest = paths.cssDest + "styles.min.css";

gulp.task("clean:js", function (cb) {
    rimraf(paths.jsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.cssDest, cb);
});

gulp.task("clean:fonts", function (cb) {
    rimraf(paths.fontsDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css", "clean:fonts"]);

gulp.task("copy:js", function () {
    return gulp.src(paths.jsSources)
      .pipe(gulp.dest(paths.jsDest));
});

gulp.task("copy:css", function () {
    return gulp.src(paths.cssSources)
      .pipe(gulp.dest(paths.cssDest));
});

gulp.task("copy:fonts", function () {
    return gulp.src(paths.fontsSources)
      .pipe(gulp.dest(paths.fontsDest));
});

gulp.task("copy", ["copy:js", "copy:css", "copy:fonts"]);

gulp.task("min:js", function () {
    return gulp.src(paths.jsSources)
      .pipe(concat(paths.jsMinDest))
      .pipe(uglify())
      .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src(paths.cssSources)
      .pipe(concat(paths.cssMinDest))
      .pipe(cssmin())
      .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css", "copy:fonts"]);
