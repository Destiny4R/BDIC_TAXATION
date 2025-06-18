$(document).ready(function () {
    // Initialize tab state
    const tabButtons = $('#nav-tab button');

    // Prevent default tab switching behavior
    tabButtons.on('click', function (e) {
        e.preventDefault();
    });

    // Next button functionality
    $('.next-tab').click(function (e) {
        e.preventDefault();
        const $currentPane = $(this).closest('.tab-pane');

        // Validate current tab before proceeding
        if (validateTab($currentPane)) {
            const $nextPane = $currentPane.next('.tab-pane');

            if ($nextPane.length) {
                const nextTabId = $nextPane.attr('id');
                switchTab(nextTabId);
            }
        }
    });

    // Previous button functionality (no validation needed)
    $('.prev-tab').click(function (e) {
        e.preventDefault();
        const $currentPane = $(this).closest('.tab-pane');
        const $prevPane = $currentPane.prev('.tab-pane');

        if ($prevPane.length) {
            const prevTabId = $prevPane.attr('id');
            switchTab(prevTabId);
        }
    });

    // Custom tab switching function
    function switchTab(tabId) {
        // Remove active class from all tabs and buttons
        $('.tab-pane').removeClass('show active');
        tabButtons.removeClass('active');

        // Add active class to target tab and button
        $(`#${tabId}`).addClass('show active');
        $(`[data-bs-target="#${tabId}"]`).addClass('active');

        // Trigger Bootstrap's fade animation
        $(`#${tabId}`).addClass('fade').addClass('show');
    }

    // Enhanced validation function
    function validateTab(tabPane) {
        let isValid = true;
        const $inputs = tabPane.find('input, select, textarea[required]');

        // Check each required field
        $inputs.each(function () {
            const $field = $(this);

            // Reset field state
            $field.removeClass('is-invalid');
            $field.next('.invalid-feedback').remove();

            // Check validation
            if (!this.checkValidity()) {
                // Mark field as invalid
                $field.addClass('is-invalid');

                // Add error message if doesn't exist
                if (!$field.next('.invalid-feedback').length) {
                    const errorMessage = this.validationMessage || 'This field is required';
                    $field.after(`<div class="invalid-feedback">${errorMessage}</div>`);
                }

                // Focus first invalid field
                if (isValid) {
                    $field.focus();
                    isValid = false;
                }
            }
        });

        // Add any custom validations here
        if (tabPane.attr('id') === 'nav-home') {
            // Example: Validate email format
            const $email = tabPane.find('[type="email"]');
            if ($email.length && !isValidEmail($email.val())) {
                $email.addClass('is-invalid');
                $email.after('<div class="invalid-feedback">Please enter a valid email address</div>');
                if (isValid) {
                    $email.focus();
                    isValid = false;
                }
            }
        }

        return isValid;
    }

    // Email validation helper
    function isValidEmail(email) {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }

    // Form submission handler
    $('#submit-btn').click(function (e) {
        e.preventDefault();

        // Validate all tabs
        let allValid = true;
        $('.tab-pane').each(function () {
            if (!validateTab($(this))) {
                allValid = false;
                // Switch to first invalid tab
                const tabId = $(this).attr('id');
                switchTab(tabId);
                return false; // break loop
            }
        });

        if (allValid) {
            $('form').submit();
        }
    });
});
