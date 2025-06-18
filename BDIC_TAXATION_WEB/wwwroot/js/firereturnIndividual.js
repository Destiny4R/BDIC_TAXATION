var table = null;
$(document).ready(function () {
    table = $('#fileindividualtax').DataTable({
        serverSide: true,
        processing: true,
        "searching": true,
        "paging": true,
        "info": true,
        ajax: {
            url: '/view/FiLeReturnTaxIndividual',
            type: 'POST',
            dataType: 'json'
        },

        // Column definitions
        columns: [
            {
                data: null,
                orderable: false,
                searchable: false,
                autoWidth: true,
                render: function (data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            { data: 'referenceno', "autoWidth": true },
            { data: 'session', "autoWidth": true },
            {
                "data": "createddate", "autoWidth": true,
                render: function (data, type, row) {
                    if (data != null) {
                        var hours = new Date(data).getHours()
                        let ap = hours >= 12 ? 'pm' : 'am';
                        return data = data.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
                    } else {
                        return "null";
                    }
                }
            },
            { data: 'assetname', "autoWidth": true },
            { data: 'income', "autoWidth": true },
            { data: 'taxamount', "autoWidth": true },
            {
                "data": "status", "autoWidth": true,
                render: function (data, type, row) {
                    if (data) {
                        return '<span class="badge badge-success"><i class="far fa-thumbs-up"></i></span>';
                    } else {
                        return '<span class="badge badge-warning"><i class="far fa-thumbs-down"></i></span>';
                    }
                }
            },
            {
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    // Add action buttons
                    return '<button type="button" data-id="' + row.id + '" class="btn btn-secondary btn-sm rounded-5 viewdetail-btn" data-bs-toggle="dropdown"><i class="ti-eye" ></i> &nbsp; View</button> &nbsp;'+
                        '<button type="button" data-id="' + row.id + '" class="btn btn-sm btn-danger delete-btn" data-bs-toggle="dropdown"><i class="far fa-trash-alt"></i></button>';
                }
            }
        ],

        // Language configuration
        language: {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span>',
            lengthMenu: 'Show _MENU_ entries',
            zeroRecords: 'No matching records found',
            info: 'Showing _START_ to _END_ of _TOTAL_ entries',
            infoEmpty: 'Showing 0 to 0 of 0 entries',
            infoFiltered: '(filtered from _MAX_ total entries)',
            search: 'Search:',
            paginate: {
                first: '<i class="fas fa-angle-left"></i>',
                last: '<i class="fas fa-angle-right"></i>',
                next: 'Next',
                previous: 'Prev'
            }
        },

        // Custom initialization
        initComplete: function () {
            // Add any initialization code here
            console.log('Table initialized');
        }
    });

    flatpickr("#fileReturn_SessionYear", {
        dateFormat: "Y"
    });
});

$(document).on('click', '.delete-btn', function () {
    const id = $(this).data('id');
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to recover this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: '/v1/DeleteFileTaxReturnIndividual/' + id,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
                        table.ajax.reload(null, false);
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: data.message,
                            footer: 'Message'
                        });
                    }
                },
                beforeSend: function () {
                    blockcallback();
                },
                error: function () {
                    $.unblockUI();
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong!',
                        footer: 'Check internet connectivity'
                    });
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Specialization not deleted',
                'error'
            );
        }
    });

});

$(document).on('click', '.viewdetail-btn', function () {
    const id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/v1/GetFileReturnIndividual/' + id,
        success: function (data) {
            $.unblockUI();
            if (data.success) {
                var dx = data.data;
                $('#income').text(dx.income);
                $('#assetname').text(dx.assetname);
                $('#session').text(dx.session);
                $('#assettaxable').text(dx.assettaxable);
                $('#reference').text(dx.referenceno);
                $('#taxamount').text(dx.taxamount);
                $('#percent').text(dx.percent);
                $('#fileId').text(dx.id);
                var hours = new Date(dx.createddate).getHours()
                let ap = hours >= 12 ? 'pm' : 'am';
                let date = dx.createddate.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
                $('#createdate').text(date);
                const $statusDiv = $('#thestatusdiv').empty(); // Clear previous content
                //
                if (dx.status) {
                    $statusDiv.append('<span class="badge badge-success"><i class="fas fa-check-double"></i></span>');
                } else {
                    $statusDiv.append('<span class="badge badge-danger"><i class="fas fa-times"></i></span>');
                }
                const $payDiv = $('#addatag').empty();
                if (dx.status) {
                    if (!dx.transactionstate) { 
                        $payDiv.append('<a href="/Tax-Payer/Proceed-Payment?id=' + dx.id + '" class="btn btn-primary"><i class="fas fa-money-check"></i> Pay Now</a>');
                    }
                }
                //Adding flag message to view
                const $msgDiv = $('#rejection').empty();
                if (typeof dx.message === 'string' && dx.message.trim() !== "") {
                    $msgDiv.append('<label>Flagged Message</label><p class="text-danger">'
                        + dx.message +
                        ' </p><strong class="mt-2">If you require further clarification, you may raise an objection for re-evaluation.</strong>'
                        + '<div class=" mt-2"><a class="btn btn-outline-warning w-100"  href="/Tax-Payer/objections?id=' + dx.id+'">Raise an objection</a></div>'
                    );
                }
                //$('#status').text(data.data.status);
                const view = new bootstrap.Modal('#ViewReturnFileTaxModal');
                view.show();

            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: data.message,
                    footer: 'Message'
                });
            }
        },
        beforeSend: function () {
            blockcallback();
        },
        error: function () {
            $.unblockUI();
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
                footer: 'Check internet connectivity'
            });
        },
        complete: function () {
            $.unblockUI();
        }
    });

});
function blockcallback() {
    $.blockUI({
        message: '<div class="bs-spinner mt-4 mt-lg-0"><div class= "spinner-border text-success mr-2 mt-2" style="width: 10rem; height: 10rem;" role="status"><span class="sr-only">Loading...</span></div> ',
        fadeIn: 800,
        overlayCSS: {
            backgroundColor: '#1b2024',
            opacity: 0.8,
            zIndex: 1200,
            cursor: 'wait'
        },
        css: {
            border: 0,
            color: '#fff',
            zIndex: 1201,
            padding: 0,
            backgroundColor: 'transparent'
        },
        onBlock: function () {

        }
    });
}