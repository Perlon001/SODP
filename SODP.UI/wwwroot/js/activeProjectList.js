var myTable;
$(document).ready(function () {
    myTable = $('#ProjectDataTable').DataTable({
        //ajax: {
        //    url: "/api/Projects",
        //    type: "GET",
        //    dataSrc: 'data.collection',
        //},
        data: ProjectsArray,
        columns: [
            { data: "number" },
            { data: "stageSign" },
            {
                data: "title",
                render: function (data, type, row) {
                    return `<a href="/ActiveProjects/CreateUpdate?id=${row.id}">${row.title}</a>`;
                }
            },
            {
                data: "id",
                render: function (data) {
                    return RenderButton(data);
                },
                width: "15%",
                visible: RoleCheck
            }
        ]
    });
});

function RenderButton(data) {
    var renderTags = `<div class="row group justify-content-center">`;
    renderTags += `<a onclick = 'Delete("/api/ActiveProjects/${data}")' class='btn btn-sm btn-danger text-white mb-0 mt-0 ml-1 mr-1 p-1' style = "cursor:pointer; width=70px;" > <i class="far fa-trash-alt"></i></a >`;
    renderTags += `<a onclick = 'Archive("/api/ActiveProjects/${data}")' class='btn btn-sm btn-success text-white mb-0 mt-0 ml-1 mr-1 p-1' style = "cursor:pointer; width=70px;" > <i class="fas fa-archive"></i></a >`;
    renderTags += `</div >`;

    return renderTags
}

function Reload() {
    var container = document.getElementById("ProjectTable");
    var content = container.innerHTML;
    container.innerHTML = content;

    // this line is to watch the result in console , you can remove it later	
    // window.location.replace("/Projects");
}
