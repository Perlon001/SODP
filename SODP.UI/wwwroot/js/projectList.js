
var myTable;
$(document).ready(function () {
    $(document).ready(function () {
        myTable = $('#ProjectDataTable').DataTable({
            ajax: {
                url: "/api/Projects",
                type: "GET",
                dataSrc: 'data.collection',
            },
            columns: [
                { data: "number" },
                { data: "stageSign" },
                { data: "title" },
                {
                    data: "id",
                    render: function (data) {
                        return `<div class="row group justify-content-center">
                                <a href='/Projects/CreateUpdate?id=${data}' class='btn btn-sm btn-info text-white mb-0 mt-0 ml-1 mr-1 p-1' style="cursor:pointer; width=70px;"><i class="far fa-edit"></i></a>
                                <a onclick='Delete("/api/Projects/${data}")' class='btn btn-sm btn-danger text-white mb-0 mt-0 ml-1 mr-1 p-1' style="cursor:pointer; width=70px;"><i class="far fa-trash-alt"></i></a>
                            </div>`;
                    },
                    width: "15%"
                }
            ]
        });
    });
});

function reload() {
    var container = document.getElementById("ProjectTable");
    var content = container.innerHTML;
    container.innerHTML = content;

    //this line is to watch the result in console , you can remove it later	
    window.location.replace("/Projects");
}
