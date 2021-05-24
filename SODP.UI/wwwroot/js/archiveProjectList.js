var myTable;
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip()

    InitProjectDataTable();
});

function InitProjectDataTable() {
    $('#ProjectDataTable').DataTable({
        data: ProjectsArray,
        columns: [
            { data: "number" },
            { data: "stageSign" },
            { data: "title" },
            {
                data: "id",
                render: function (data) {
                    var renderTags = `<div class="row group justify-content-center">`;
                    renderTags += `<a onclick = 'Restore("/api/ArchiveProjects/${data}")' class='btn btn-sm btn-success text-white mb-0 mt-0 ml-1 mr-1 p-1' style = "cursor:pointer; width=70px;" data-toggle="tooltip" data-placement="top" title="Przywróć projekt do aktywnych"> <i class="fas fa-archive"></i></a >`;
                    renderTags += `</div >`;

                    return renderTags;
                },
                width: "15%",
                visible: RoleCheck
            }
        ]
    });
}

function Reload() {
    var container = document.getElementById("ProjectTable");
    var content = container.innerHTML;
    container.innerHTML = content;

    // this line is to watch the result in console , you can remove it later	
    // window.location.replace("/Projects");
}
