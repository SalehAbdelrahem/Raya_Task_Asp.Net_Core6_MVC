$(document).ready(function () {
    $('#Roles').dataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Roles/GetAllRoles",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            { "data": "id", "name": "Id", "autowidth": true },
            { "data": "name", "name": "Name", "autowidth": true },
            
            {
                "render": function (data, type, row) {
                    return ` <div class="btn-group" role="group" aria-label="Basic mixed styles example">
                            <a onclick=Edit("${row.id}")  type="button" class="btn btn-warning"><i style="font-size:smaller" class="mx-1 fa-solid fa-pen-to-square"></i>Edit</a>
                            <a onclick=Details("${row.id}") type="button" class="btn btn-info"><i style="font-size:smaller" class="mx-1 fa-solid fa-eye"></i>Details</a>
                            <a onclick=Delete("${row.id}") type="button" class="delete btn btn-danger "><i style="font-size:smaller" class="mx-1 fa-sharp fa-solid fa-trash"></i>Delete</a>     </div>`
 },
                "orderable": false
            },
            
        ]
    });


});

function Delete(id) {
    swal({

        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this data!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/Roles/Delete?id=" + id,
                    type: "POST",
                    success: function () {
                        swal("Poof! Your data has been deleted!", {
                            icon: "success",
                        }).then((result) => {
                            location.reload();
                        });
                    },
                    error: function () {
                        swal("Oops", "We couldn't connect to the server!", "error");
                    }
                });
            } else {
                swal("Your data is safe!");
            }
        });
}

function Details(id) {
    window.location = `/Roles/Details?id=${id}`;

}
function Edit(id) {
    window.location = `/Roles/Edit?id=${id}`;
}