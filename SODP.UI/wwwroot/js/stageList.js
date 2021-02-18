var myTable;
$(document).ready(function () {
    $(document).ready(function () {
        myTable = $('#StageDataTable').DataTable({
            ajax: {
                url: "/api/Stages",
                type: "GET",
                dataSrc: 'data.collection',
            },
            columns: [
                { data: "sign" },
                { data: "description" },
                {
                    data: "sign",
                    render: function (data) {
                        return `<div class="row group justify-content-center">
                                <a href='/Stages/CreateUpdate?sign=${data}' class='btn btn-sm btn-info text-white mb-0 mt-0 ml-1 mr-1 p-1' style="cursor:pointer; width=70px;"><i class="far fa-edit"></i></a>
                                <a onclick='Delete("/api/Stages/${data}")' class='btn btn-sm btn-danger text-white mb-0 mt-0 ml-1 mr-1 p-1' style="cursor:pointer; width=70px;"><i class="far fa-trash-alt"></i></a>
                            </div>`;
                    },
                    width: "15%"
                }
            ]
        });
    });
});

function Reload() {
    //var container = document.getElementById("StageTable");
    //var content = container.innerHTML;
    //container.innerHTML = content;

    ////this line is to watch the result in console , you can remove it later	
    window.location.reload(true);
}