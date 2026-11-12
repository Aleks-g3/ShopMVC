(function () {
    function focusFirstValidationError() {
        const invalidFields = document.querySelectorAll(".input-validation-error");
        invalidFields.forEach(field => field.setAttribute("aria-invalid", "true"));

        if (invalidFields.length > 0) {
            const firstInvalidField = invalidFields[0];
            firstInvalidField.focus();
            firstInvalidField.scrollIntoView({ block: "center" });
            return;
        }

        const summary = document.querySelector(".validation-summary-errors");
        if (summary) {
            summary.setAttribute("tabindex", "-1");
            summary.focus();
        }
    }

    if (document.readyState === "loading")
        document.addEventListener("DOMContentLoaded", focusFirstValidationError);
    else
        focusFirstValidationError();
})();
