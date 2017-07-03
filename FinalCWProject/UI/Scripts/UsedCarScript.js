var city;
var minPrice;
var maxPrice;

$(document).ready(function () {
    ReadFilterValue();
    if (CheckBudget(minPrice, maxPrice)) {
        var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "";
        $("#showCars").load(path);
    }
    else {
        minPrice = "";
        maxPrice = "";
        var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "";
        $("#showCars").load(path);
    }

    $('.cityFilter').change(function () {
        ReadFilterValue();
        var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "";
        $.ajax(path).done(function (response) {
            console.log(response);
            $("#showCars").html(response);
        })
    });


    $("#filterByBudgetButton").click(function () {
        ReadFilterValue();
        if (minPrice == "") {
            var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "";
            $("#showCars").load(path);
        }
        else {
            var minValue = parseInt(minPrice);
            var maxValue = parseInt(maxPrice);

            if (CheckBudget(minValue, maxValue)) {
                var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "";
                $("#showCars").load(path);
            }
            else {
                alert("Enter Value between 10k to 5cr");
                $("#minPrice").val(null);
                $("#maxPrice").val(null);
            }
        }
    });

});

function ReadFilterValue() {
    minPrice = $("#minPrice").val();
    maxPrice = $("#maxPrice").val();
    city = $("#cities option:selected").text().toLowerCase();
}

function CheckBudget(minValue, maxValue) {
    if ((minValue > 10000 && minValue <= maxValue && maxValue < 50000000)) {

        return 1;
    }
    else {
        return 0;
    }
}

function ChangePage(buttonPress) {
    $(document).ready(function () {
        var page;
        if (buttonPress == -1)
            page = $("#previousButton1").attr("value");
        else
            page = $("#nextButton1").attr("value");
        var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "&page=" + page + "";
        $("#showCars").load(path);
    });
}

function ProfilePage(stockID) {
    $(document).ready(function () {
        var currentUrl = window.location.href;
        var url;
        if (currentUrl.search("Home") != -1)
            url = currentUrl+"/stock/" + stockID + "";
        else
            url = "/Home/stock/" + stockID + "";
        $(location).attr('href', url);
    });
}
