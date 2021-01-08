$(document).ready(() => {
    ShowAllResources();
    function ShowAllResources() {
        $("table tbody").html("");
        $.ajax({
            url: "https://localhost:44364/api/resources",
            type: "get",
            contentType: "application/json",
            headers: { 'Access-Control-Allow-Origin': 'https://localhost:44377/AllResources.html' },
            success: function (result, status, xhr) {
                console.log(xhr);
                $.each(result, function (index, value) {
                    $("tbody").append($("<tr>"));
                    appendElement = $("tbody tr").last();
                    appendElement.append($("<td>").html(value["resourceId"]).hide());
                    appendElement.append($("<td>").html(value["lastname"]));
                    appendElement.append($("<td>").html(value["firstname"]));
                    appendElement.append($("<td>").html(value["middlename"]));
                    appendElement.append($("<td>").html(new Date(value["hiredate"]).toISOString().slice(0, 10)));
                    appendElement.append($("<td>").html(new Date(value["termdate"]).toISOString().slice(0, 10)));
                    //appendElement.append($("<td>").html("<a href=\"UpdateResource.html\" class=\"btn btn-primary m-2\" id=\"Edit\">Edit</a>"))
                    appendElement.append($("<td>").html("<a href=\"#\" class=\"btn btn-primary m-2\" id=\"Edit\">Edit</a>"))
                    appendElement.append($("<td>").html("<a href=\"#\" class=\"btn btn-danger m-2\" id=\"Delete\">Delete</a>"))
                });
            },
            error: function (xhr, status, error) {
                console.log(xhr);
            }

        });
        $("table").on("click", "a#Delete", function () {
            var resourceId = $(this).parents("tr").find("td:nth-child(1)").text();

            $.ajax({
                url: "https://localhost:44364/api/resources/" + resourceId,
                type: "delete",
                contentType: "application/json",
                success: function (result, status, xhr) {
                    ShowAllResources();
                },
                error: function (xhr, status, error) {
                    console.log(xhr)
                }
            });
        });
        
    }
    
    

});