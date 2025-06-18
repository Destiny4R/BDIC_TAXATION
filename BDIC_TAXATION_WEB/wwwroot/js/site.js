//$(document).ready(function () {
//    // Initialize tab state
//    const tabButtons = $('#nav-tab button');

//    // Prevent default tab switching behavior
//    tabButtons.on('click', function (e) {
//        e.preventDefault();
//    });

//    // Next button functionality
//    $('.next-tab').click(function (e) {
//        e.preventDefault();
//        const $currentPane = $(this).closest('.tab-pane');

//        // Validate current tab before proceeding
//        if (validateTab($currentPane)) {
//            const $nextPane = $currentPane.next('.tab-pane');

//            if ($nextPane.length) {
//                const nextTabId = $nextPane.attr('id');
//                switchTab(nextTabId);
//            }
//        }
//    });

//    // Previous button functionality (no validation needed)
//    $('.prev-tab').click(function (e) {
//        e.preventDefault();
//        const $currentPane = $(this).closest('.tab-pane');
//        const $prevPane = $currentPane.prev('.tab-pane');

//        if ($prevPane.length) {
//            const prevTabId = $prevPane.attr('id');
//            switchTab(prevTabId);
//        }
//    });

//    // Custom tab switching function
//    function switchTab(tabId) {
//        // Remove active class from all tabs and buttons
//        $('.tab-pane').removeClass('show active');
//        tabButtons.removeClass('active');

//        // Add active class to target tab and button
//        $(`#${tabId}`).addClass('show active');
//        $(`[data-bs-target="#${tabId}"]`).addClass('active');

//        // Trigger Bootstrap's fade animation
//        $(`#${tabId}`).addClass('fade').addClass('show');
//    }

//    // Enhanced validation function
//    function validateTab(tabPane) {
//        let isValid = true;
//        const $inputs = tabPane.find('input, select, textarea[required]');

//        // Check each required field
//        $inputs.each(function () {
//            const $field = $(this);

//            // Reset field state
//            $field.removeClass('is-invalid');
//            $field.next('.invalid-feedback').remove();

//            // Check validation
//            if (!this.checkValidity()) {
//                // Mark field as invalid
//                $field.addClass('is-invalid');

//                // Add error message if doesn't exist
//                if (!$field.next('.invalid-feedback').length) {
//                    const errorMessage = this.validationMessage || 'This field is required';
//                    $field.after(`<div class="invalid-feedback">${errorMessage}</div>`);
//                }

//                // Focus first invalid field
//                if (isValid) {
//                    $field.focus();
//                    isValid = false;
//                }
//            }
//        });

//        // Add any custom validations here
//        if (tabPane.attr('id') === 'nav-home') {
//            // Example: Validate email format
//            const $email = tabPane.find('[type="email"]');
//            if ($email.length && !isValidEmail($email.val())) {
//                $email.addClass('is-invalid');
//                $email.after('<div class="invalid-feedback">Please enter a valid email address</div>');
//                if (isValid) {
//                    $email.focus();
//                    isValid = false;
//                }
//            }
//        }

//        return isValid;
//    }

//    // Email validation helper
//    function isValidEmail(email) {
//        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
//    }

//    // Form submission handler
//    $('#submit-btn').click(function (e) {
//        e.preventDefault();

//        // Validate all tabs
//        let allValid = true;
//        $('.tab-pane').each(function () {
//            if (!validateTab($(this))) {
//                allValid = false;
//                // Switch to first invalid tab
//                const tabId = $(this).attr('id');
//                switchTab(tabId);
//                return false; // break loop
//            }
//        });

//        if (allValid) {
//            $('form').submit();
//        }
//    });
//});

$(document).ready(function () {
    // Store tab elements for easy access
    const tabs = $('.nav-tabs button');
    const tabPanes = $('.tab-pane');

    // Handle next button click
    $('.next-tab').click(function () {
        const currentTab = $(this).closest('.tab-pane');
        const currentTabId = currentTab.attr('id');
        let isValid = true;

        // Validate fields in the current tab before proceeding
        currentTab.find('.required').each(function () {
            if ($(this).is(':checkbox')) {
                if (!$(this).is(':checked')) {
                    isValid = false;
                    $(this).addClass('is-invalid');
                } else {
                    $(this).removeClass('is-invalid');
                }
            } else {
                if (!$(this).val()) {
                    isValid = false;
                    $(this).addClass('is-invalid');
                } else {
                    $(this).removeClass('is-invalid');
                }
            }
        });

        // Special check for password match
        if (currentTabId === 'nav-home') {
            const password = $('input[name="BusinessTax.Password"]').val();
            const confirmPassword = $('#confirmpassword').val();

            if (password !== confirmPassword) {
                isValid = false;
                $('#confirmpassword').addClass('is-invalid');
                $('input[name="BusinessTax.Password"]').addClass('is-invalid');
                $('span[data-valmsg-for="BusinessTax.ConfirmPassword"]').text('Passwords do not match');
            } else {
                $('#confirmpassword').removeClass('is-invalid');
                $('input[name="BusinessTax.Password"]').removeClass('is-invalid');
                $('span[data-valmsg-for="BusinessTax.ConfirmPassword"]').text('');
            }
        }

        if (isValid) {
            // Find the next tab
            const currentIndex = tabPanes.index(currentTab);
            if (currentIndex < tabPanes.length - 1) {
                const nextTabId = tabPanes.eq(currentIndex + 1).attr('id');

                // Update tab navigation
                tabs.each(function () {
                    $(this).removeClass('active');
                    if ($(this).attr('data-bs-target') === `#${nextTabId}`) {
                        $(this).addClass('active');
                    }
                });

                // Show next tab content
                tabPanes.each(function () {
                    $(this).removeClass('show active');
                    if ($(this).attr('id') === nextTabId) {
                        $(this).addClass('show active');
                    }
                });
            }
        }
    });

    // Handle previous button click
    $('.prev-tab').click(function () {
        const currentTab = $(this).closest('.tab-pane');
        const currentIndex = tabPanes.index(currentTab);

        if (currentIndex > 0) {
            const prevTabId = tabPanes.eq(currentIndex - 1).attr('id');

            // Update tab navigation
            tabs.each(function () {
                $(this).removeClass('active');
                if ($(this).attr('data-bs-target') === `#${prevTabId}`) {
                    $(this).addClass('active');
                }
            });

            // Show previous tab content
            tabPanes.each(function () {
                $(this).removeClass('show active');
                if ($(this).attr('id') === prevTabId) {
                    $(this).addClass('show active');
                }
            });
        }
    });

    // Enable submit button only if terms checkbox is checked
    $('#invalidCheck').change(function () {
        $('#submit-btn').prop('disabled', !$(this).is(':checked'));
    }).trigger('change');

    // Form submission handling
    $('form').submit(function (e) {
        // Final validation before submission
        let allValid = true;
        $('.required').each(function () {
            if ($(this).is(':checkbox')) {
                if (!$(this).is(':checked')) {
                    allValid = false;
                    $(this).addClass('is-invalid');
                }
            } else {
                if (!$(this).val()) {
                    allValid = false;
                    $(this).addClass('is-invalid');
                }
            }
        });

        if (!allValid) {
            e.preventDefault();
            // Scroll to the first invalid field
            $('html, body').animate({
                scrollTop: $('.is-invalid').first().offset().top - 100
            }, 500);
        }
    });
});