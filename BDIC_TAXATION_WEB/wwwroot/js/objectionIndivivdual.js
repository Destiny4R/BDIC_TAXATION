var table = null;
$(document).ready(function () {
    table = $('#objectiontable').DataTable({
        serverSide: true,
        processing: true,
        "searching": true,
        "paging": true,
        "info": true,
        ajax: {
            url: '/view/taxObjectionsIndividual',
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
            { data: 'reference', "autoWidth": true },
            {
                "data": "createdate", "autoWidth": true,
                render: function (data, type, row) {
                    return "N/A";
                }
            },
            { data: 'payerid', "autoWidth": true },
            { data: 'income', "autoWidth": true },
            { data: 'taxamount', "autoWidth": true },
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
            {
                "data": "objectiontype", "autoWidth": true,
                render: function (data, type, row) {
                    return getSwitchLabel(data)
                }
            },
            {
                data: null,   orderable: false,  render: function (data, type, row) {
                    // Create dropdown container
                    const dropdown = document.createElement('div');
                    dropdown.className = 'dropdown';

                    // Create dropdown button
                    const button = document.createElement('button');
                    button.type = 'button';
                    button.className = 'btn btn-light';
                    button.setAttribute('data-bs-toggle', 'dropdown');
                    button.innerHTML = '<i class="mdi mdi-dots-horizontal"></i>';

                    // Create dropdown menu
                    const menu = document.createElement('ul');
                    menu.className = 'dropdown-menu';

                    // View item
                    const viewItem = document.createElement('li');
                    const viewLink = document.createElement('a');
                    viewLink.href = '#';
                    viewLink.className = 'dropdown-item viewdetail-btn';
                    viewLink.setAttribute('data-id', row.id);
                    viewLink.setAttribute('data-bs-toggle', 'modal');
                    viewLink.setAttribute('data-bs-target', '#objectionsummaryModal');
                    viewLink.innerHTML = '<i class="ti-eye"></i> View';
                    viewItem.appendChild(viewLink);

                    // Cancel item
                    const cancelItem = document.createElement('li');
                    const cancelLink = document.createElement('a');
                    cancelLink.href = '#';
                    cancelLink.className = 'dropdown-item btn-closed';
                    cancelLink.setAttribute('data-id', row.id);
                    cancelLink.innerHTML = '<i class="bi bi-x"></i> Cancel';
                    cancelItem.appendChild(cancelLink);

                    // Append items to menu
                    menu.appendChild(viewItem);
                    menu.appendChild(cancelItem);

                    // Append button and menu to dropdown
                    dropdown.appendChild(button);
                    dropdown.appendChild(menu);

                    return dropdown.outerHTML;
                }
            }
        ],

        // Language configuration
        language: {
            processing: '<div class="spinner-border text-success" style="width: 8rem; height: 8rem;" role="status"><span class="visually-hidden">Loading...</span></div>',
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

$(document).on('click', '.viewdetail-btn', function () {
    const id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/v1/GetObjectionDetailsIndividual/' + id,
        success: function (data) {
            $.unblockUI();
            if (data.success) {
                var dx = data.data;
                $('#income').text(dx.income);
                $('#name').text(dx.name);
                $('#session').text(dx.session);
                $('#payerid').text(dx.payerid);
                $('#reference').text(dx.reference);
                $('#message').text(dx.objectiontype);
                $('#message').text(dx.message);
                $('#taxamount').text(dx.taxamount);
                $('#status').text(dx.status);
                $('#reason').text(dx.reason);
                var hours = new Date(dx.createddate).getHours()
                let ap = hours >= 12 ? 'pm' : 'am';
                let date = dx.createdate.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
                $('#createdate').text(date);
                const $statusDiv = $('#thestatusdiv').empty(); // Clear previous content
                //
                $statusDiv.append(getSwitchLabel(dx.objectiontype));
                const $payDiv = $('#addatag').empty();
                if (dx.status) {
                    if (!dx.transactionstate) { 
                        $payDiv.append('<a href="/Tax-Payer/Proceed-Payment?id=' + dx.id + '" class="btn btn-primary"><i class="fas fa-money-check"></i> Pay Now</a>');
                    }
                }
                //ADD FILE DOWNLOAD DIV
                let divData = `<a href="${dx.filepath}" class="col-12" target="_blank">
                                        <div class="">
                                    <i class="bi bi-file-earmark-pdf fs-1 text-danger"></i>
                                    <p class="small mb-1">${dx.filename}</p>
                                </div>
                                </a>`;
                const $divData = $('#btn-downloadfile').empty();
                $divData.append(divData);
                //Adding flag message to view
                //const $msgDiv = $('#rejection').empty();
                //if (typeof dx.message === 'string' && dx.message.trim() !== "") {
                //    $msgDiv.append('<label>Flagged Message</label><p class="text-danger">'
                //        + dx.message +
                //        ' </p><strong class="mt-2">If you require further clarification, you may raise an objection for re-evaluation.</strong>'
                //        + '<div class=" mt-2"><a class="btn btn-outline-warning w-100"  href="/Tax-Payer/objections?id=' + dx.id+'">Raise an objection</a></div>'
                //    );
                //}
                //$('#status').text(data.data.status);
                const view = new bootstrap.Modal('#objectionsummaryModal');
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
//ACTIONS

$(".input-mask").inputmask();

var incomingid = 0;
incomingid = $('#incomingId').val();
if (incomingid > 0) {
    const view = new bootstrap.Modal('#objectionModal');
    view.show();
}

document.addEventListener('DOMContentLoaded', () => {
    const dropZone = document.getElementById('dropZone');
    const fileDetails = document.getElementById('fileDetails');
    const hiddenFileInput = document.getElementById('hiddenFileInput'); // This is the single file input for submission


    // Handle drag events
    dropZone.addEventListener('dragover', (e) => {
        e.preventDefault();
        dropZone.classList.add('dragover');
    });

    dropZone.addEventListener('dragleave', () => {
        dropZone.classList.remove('dragover');
    });

    // Handle file drop events
    dropZone.addEventListener('drop', (e) => {
        e.preventDefault();
        dropZone.classList.remove('dragover');
        const files = e.dataTransfer.files;
        if (files.length) {
            handleFileUpload(files[0]); // Process the first dropped file
        }
    });

    // Allow clicking the drop zone to open the file dialog
    dropZone.addEventListener('click', () => {
        // Use the hidden file input for the native file dialog
        hiddenFileInput.click();
    });

    // Update file when user selects file via file dialog
    hiddenFileInput.addEventListener('change', () => { // No need for 'e' parameter here
        if (hiddenFileInput.files.length) {
            handleFileUpload(hiddenFileInput.files[0]); // Process the selected file
        }
    });

    // Validate file and update the hidden input's files property
    function handleFileUpload(file) {
        const maxSize = 5 * 1024 * 1024; // 5MB limit

        if (file.size > maxSize) {
            alert('File size exceeds 5MB limit.');
            // Clear the input if the file is too large
            hiddenFileInput.value = "";
            fileDetails.innerHTML = ''; // Clear displayed file details
            return;
        }

        // The core "transfer" logic: assign the selected/dropped file to the hiddenFileInput
        const dataTransfer = new DataTransfer();
        dataTransfer.items.add(file);
        hiddenFileInput.files = dataTransfer.files;

        displayFileDetails(file);
    }

    // Display file details with a remove option
    function displayFileDetails(file) {
        fileDetails.innerHTML = `
            <div class="alert alert-info d-flex justify-content-between align-items-center mt-2">
                <span>${file.name} (${(file.size / 1024).toFixed(2)} KB)</span>
                <button type="button" class="btn btn-sm btn-light" id="removeFile"><i class="far fa-times-circle"></i></button>
            </div>
        `;
        document.getElementById('removeFile').addEventListener('click', () => {
            // Clear the hidden file input
            hiddenFileInput.value = ""; // This is the simplest way to clear a file input
            fileDetails.innerHTML = ''; // Clear displayed details
            console.log("File removed. Input is cleared.");
        });
    }
});
function getSwitchLabel(data) {
    switch (data) {
        case "0":
            return '<span class="badge badge-warning">Under review</span>';
        case "1":
            return '<span class="badge bg-info">In Progress</span>';
        case "2":
            return '<span class="badgebg-success badge-status">Resolved</span>';
        case "3":
            return '<span class="badge bg-danger opacity-75">Closed</span>';
        default:
            return '<span class="badgebg-dark">N/A</span>';
    }
}

//CANCEL  OBJECTION REQUECT
$(document).on('click', '.btn-closed', function () {
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
        text: "You want to cancel this objections?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, Execute!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: '/v1/CancelObjectionIndividual/' + id,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Action Result!',
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
                'Action aborted',
                'error'
            );
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

$(document).on('click', '.btn-close', function () {
    setTimeout(() => { // Small delay to ensure cleanup
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        view.show();
    }, 500);
});