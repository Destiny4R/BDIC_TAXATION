var table = null;
$(document).ready(function () {
    table = $('#taxofficesTable').DataTable({
        serverSide: true,
        processing: true,
        "searching": true,
        "paging": true,
        "info": true,
        "responsive": true,
        ajax: {
            url: '/view/TaxOfficesViews',
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
            { data: 'name', "autoWidth": true },
            { data: 'code', "autoWidth": true },
            { data: 'lga', "autoWidth": true },
            {
                "data": "status", "autoWidth": true,
                render: function (data, type, row) {
                    if (data) {
                        return '<span class="badge badge-success"><i class="fas fa-chevron-down"></i> Active</span>';
                    } else {
                        return '<span class="badge badge-warning"><i class="fas fa-chevron-down"></i> Deactivated</span>';
                    }
                }
            },
            {
                "data": "createdate", "autoWidth": true,
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
            { data: 'officerincharge', "autoWidth": true },
            {
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    // Add action buttons
                    return '<button type="button" data-id="' + row.id + '" class="btn btn-secondary btn-sm rounded-5 btn-viewdata" data-bs-target="#manageOfficeModal" data-bs-toggle="modal"><i class="ti-settings" ></i> &nbsp; View</button> &nbsp;';
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

});

$(document).on('click', '.delete-btn', function () {
    //const id = $(this).data('id');
    var id = $('#caseId').val();
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
                url: '/v1/DeleteFileTaxOffices/' + id,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
                        table.ajax.reload(null, false);
                        $('#manageOfficeModal').modal('hide');
                        setTimeout(() => { // Small delay to ensure cleanup
                            $('body').removeClass('modal-open');
                            $('.modal-backdrop').remove();
                            //view.show();
                        }, 500);
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
                'Tax Office not deleted',
                'error'
            );
        }
    });

});
//DISABLED  OR ENABLED TAX OFFICE
$(document).on('click', '.btn-enable-Disable', function () {
    //const id = $(this).data('id');
    var id = $('#caseId').val();
    const msg = $('#message').val();
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: msg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, Execute!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: '/v1/DisableTaxOffice/' + id,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Action Result!',
                            data.message,
                            'success'
                        );
                        table.ajax.reload(null, false);
                        $('#manageOfficeModal').modal('hide');
                        setTimeout(() => { // Small delay to ensure cleanup
                            $('body').removeClass('modal-open');
                            $('.modal-backdrop').remove();
                            //view.show();
                        }, 500);
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
                'Action aborted',
                'error'
            );
        }
    });

});
//GET DATA FOR UPDATING/EDITTING
//Get Data For Updating
$(document).on('click', '.btn-editdata', function () {
    //const id = $(this).data('id');
    var id = $('#caseId').val();
     $.ajax({
        type: "POST",
        url: '/v1/GetTaxOfficedata/' + id,
        success: function (data) {
            $.unblockUI();
            if (data.success) {
                $('#taxOffice2_Name').val(data.data.name);
                $('#taxOffice2_code').val(data.data.code);
                $('#taxOffice2_Address').val(data.data.address);
                $('#taxOffice2_Phone').val(data.data.phone);
                $('#taxOffice2_Email').val(data.data.email);
                $('#taxOffice2_LgaId').val(data.data.lgaId);
                $('#taxOffice2_OfficerInCharge').val(data.data.officerincharge);
                $('#taxOffice2_Id').val(data.data.id);
                const view = new bootstrap.Modal('#editTaxOfficeModal');

                $('#manageOfficeModal').modal('hide');
                setTimeout(() => { // Small delay to ensure cleanup
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();
                    view.show();
                }, 500); // Adjust delay if needed
                
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

//Get Data For VIEWING
$(document).on('click', '.btn-viewdata', function () {
    const id = $(this).data('id');
    
    $.ajax({
        type: "POST",
        url: '/v1/GetTaxOfficedata/' + id,
        success: function (data) {
            $.unblockUI();
            if (data.success) {
                $('#name').text(data.data.name);
                $('#code').text(data.data.code);
                $('#address').text(data.data.address);
                $('#phone').text(data.data.phone);
                $('#email').text(data.data.email);
                $('#lga').text(data.data.lga);
                $('#officer').text(data.data.officerincharge);
                $('#id').text(data.data.id);
                var hours = new Date(data.data.createdate).getHours()
                let ap = hours >= 12 ? 'pm' : 'am';
                let date = data.data.createdate.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
                $('#createdate').text(date);
                const $statusDiv = $('#thestatusdiv').empty(); // Clear previous content

                if (data.data.status) {
                    $statusDiv.append('<span class="badge badge-success"><i class="fas fa-check-double"></i> Active</span>');
                } else {
                    $statusDiv.append('<span class="badge badge-danger"><i class="fas fa-times"></i> Deactivated</span>');
                }
                //$('.delete-btn').data('id', data.data.id);
                $('.delete-btn').attr('data-id', data.data.id);
                $('.btn-enable-Disable').attr('data-id', data.data.id);
                $('.btn-editdata').attr('data-id', data.data.id);
                $('#caseId').val(data.data.id);
                updateButtonState(data.data.status);
                const view = new bootstrap.Modal('#manageOfficeModal');
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
$(".input-mask").inputmask();
function updateButtonState(action) {
    const button = $('#activedeactive');
    const msg = $('#message');
    if (action) {
        button.removeClass('btn-success').addClass('btn-warning');
        button.text('Deactivate');
        msg.val("Are you sure you want Deactivate this Office?");
    } else {
        button.removeClass('btn-warning').addClass('btn-success');
        button.text('Activate');
        msg.val("Are you sure you want Reactivate this Office?");
    }
}