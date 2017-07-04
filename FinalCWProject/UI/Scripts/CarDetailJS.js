$(document).ready(function () {
    var count = parseInt($("#image").attr("count"));
    if (count <= 1) {
        $("#nextImg").hide();
        $("#previousImg").hide();
    }
});

function PlusSlides(number) {
    var currentValue = $("#image").attr("value");
    var nextValue = parseInt(currentValue) + number;
    var imageFolder = $("#image").attr("image_id");
    var count = parseInt($("#image").attr("count"));
    if (nextValue > count) {
        nextValue = 1;
    }
    else if (nextValue < 1) {
        nextValue = count;
    }
    var imageSource = "/CarImages/" + imageFolder + "/large_" + nextValue + ".jpg";
    $("#image").attr("value", nextValue);
    $("#image").attr("src", imageSource);
}

