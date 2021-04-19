function Archive(url) {
    swal({
        title: "Czy jesteś pewien?",
        text: "Operacja nie może być cofnięta.",
        icon: "warning",
        buttons: true,
        danegerMode: true
    }).then((willArchive) => {
        console.log(willArchive);
        if (willArchive) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        window.location.reload(true);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}