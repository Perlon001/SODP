function Archive(url) {
    swal({
        title: "Czy jesteś pewien?",
        text: "Przesunięcie projektu do archiwum.",
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
                        window.location.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}