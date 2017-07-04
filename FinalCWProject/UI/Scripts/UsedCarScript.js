var city;
var minPrice;
var maxPrice;

$(document).ready(function () {
    ReadFilterValue();
    if (CheckBudget(minPrice, maxPrice)) {
        LoadPath();
    }
    else {
        minPrice = "";
        maxPrice = "";
        $("#minPrice").val(null);
        $("#maxPrice").val(null);
        LoadPath();
    }

    $('.cityFilter').change(function () {
        ReadFilterValue();
        if (minPrice == "" || maxPrice == "") {
            $("#minPrice").val(null);
            $("#maxPrice").val(null);
            minPrice = "";
            maxPrice = "";
        }
        LoadPath();
    });

    $("#filterByBudgetButton").click(function () {
        ReadFilterValue();
        var minValue = parseInt(minPrice);
        var maxValue = parseInt(maxPrice);
            

        if (!CheckBudget(minValue, maxValue)) {
            if(minPrice!="" && maxPrice!="")
                alert("Enter Value between 10k to 5cr");
                $("#minPrice").val(null);
                $("#maxPrice").val(null);
                minPrice="";
                maxPrice="";
            }
        LoadPath();
        }
    );

});

function ReadFilterValue() {
    minPrice = $("#minPrice").val();
    maxPrice = $("#maxPrice").val();
    city = $("#cities option:selected").text().toLowerCase();
}

function CheckBudget(minValue, maxValue) {
    return (minValue > 10000 && minValue <= maxValue && maxValue < 50000000)
}

function LoadPath()
{
    var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "";
    $("#showCars").load(path);
}

function ChangePage(buttonPress) {
        var page;
        if (buttonPress == -1)
            page = $("#previousButton1").attr("value");
        else
            page = $("#nextButton1").attr("value");
        var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "&page=" + page + "";
        $("#showCars").load(path);
}

function ProfilePage(stockID) {
    window.location.href = "/Home/stock/" + stockID + "/";
}
