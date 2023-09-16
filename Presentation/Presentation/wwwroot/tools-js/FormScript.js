
class FormValidation {
        constructor(formSelector) {
            this.form = document.querySelector(formSelector);
            this.addSubmitEventListener();
        }

    addSubmitEventListener() {

        let self = this;

        if (!this.allFieldsValid()) {
            event.preventDefault();
            event.stopPropagation();
            //self.showValidationFeedback();
        }
        else {
            self.form.classList.add('was-validated');
        }
       
    }



    //allFieldsValid() {
    //    const elements = this.form.elements;
    //    for (let i = 0; i < elements.length; i++) {
    //        const element = elements[i];
    //        if (element.required && !element.value) {
    //            return false; // Return false if a required field is empty
    //        }

    //        // Custom validation for specific input types
    //        if (element.type === "email") {
    //            if (!this.isValidEmail(element.value)) {
    //                element.classList.add('is-invalid');
    //                element.classList.remove('is-valid');
    //                return false; // Return false if email is not valid
    //            }
    //        }

    //        if (element.type === "tel") {
    //            if (!this.isValidNigeriaMobileNumber(element.value)) {
    //                element.classList.add('is-invalid');
    //                element.classList.remove('is-valid');
    //                return false; // Return false if phone number is not valid
    //            }
    //        }
    //        // Add more custom validators for other input types as needed
    //    }
    //    return true; // All required fields have values and pass custom validation
    //}


    allFieldsValid() {
        const elements = this.form.elements;
        let isValid = true;

        for (let i = 0; i < elements.length; i++) {
            const element = elements[i];
            if (element.required) {
                if (!element.value) {
                    element.classList.add('is-invalid');
                    element.classList.remove('is-valid');
                    isValid = false;

                    const errorMessage = element.getAttribute('data-error-message');
                    if (errorMessage) {
                     
                        const errorSpan = element.nextElementSibling;
                        if (errorSpan) {
                             errorSpan.classList.add('invalid-feedback');
                            errorSpan.classList.remove('valid-feedback');
                            errorSpan.textContent = errorMessage;
                           
                        }
                    }

                } else {
                    element.classList.add('is-valid');
                    element.classList.remove('is-invalid');

                    const successSpanId = element.getAttribute('asp-frm-validation');
                    const successSpan = document.getElementById(successSpanId);
                    if (successSpan) {
                        successSpan.textContent = 'Success message'; // Replace with your success message
                        successSpan.classList.add('valid-feedback');
                        successSpan.classList.remove('invalid-feedback');
                    }
                }
            }

            // Custom validation for special cases like tel and email
            if (element.type === 'tel') {
                if (!this.isValidNigeriaMobileNumber(element.value)) {
                    element.classList.add('is-invalid');
                    element.classList.remove('is-valid');
                    isValid = false;
                } else {
                    element.classList.add('is-valid');
                    element.classList.remove('is-invalid');
                }
            }

            if (element.type === 'email') {
                if (!this.isValidEmail(element.value)) {
                    element.classList.add('is-invalid');
                    element.classList.remove('is-valid');
                    isValid = false;
                } else {
                    element.classList.add('is-valid');
                    element.classList.remove('is-invalid');
                }
            }

            element.addEventListener('input', () => {
                const errorSpanId = element.getAttribute('asp-frm-validation');
                const errorSpan = document.getElementById(errorSpanId);

                if (element.required) {
                    if (!element.value) {
                        element.classList.add('is-invalid');
                        element.classList.remove('is-valid');
                        isValid = false;

                        // Display error message from the data-error-message attribute
                        const errorMessage = element.getAttribute('data-error-message');
                        if (errorSpan) {
                            errorSpan.classList.add('invalid-feedback');
                            errorSpan.classList.remove('valid-feedback');
                            errorSpan.textContent = errorMessage || '';
                        }
                    }
                    else {
                        element.classList.add('is-valid');
                        element.classList.remove('is-invalid');

                        if (errorSpan) {
                            errorSpan.classList.add('valid-feedback');
                            errorSpan.classList.remove('invalid-feedback');
                            errorSpan.textContent = '';
                           
                        }
                    }
                }
                if (element.type === 'tel') {
                    if (!this.isValidNigeriaMobileNumber(element.value)) {
                        element.classList.add('is-invalid');
                        element.classList.remove('is-valid');
                        isValid = false;

                        // Display error message for <span> element with id="telError"
                        const errorSpanId = 'telError';
                        const errorSpan = document.getElementById(errorSpanId);
                        if (errorSpan) {
                            errorSpan.textContent = 'Invalid phone number';
                            errorSpan.classList.add('invalid-feedback');
                            errorSpan.classList.remove('valid-feedback');
                        }
                    } else {
                        element.classList.add('is-valid');
                        element.classList.remove('is-invalid');

                        // Clear error message for <span> element with id="telError"
                        const errorSpanId = 'telError';
                        const errorSpan = document.getElementById(errorSpanId);
                        if (errorSpan) {
                            errorSpan.classList.add('valid-feedback');
                            errorSpan.classList.remove('invalid-feedback');
                            errorSpan.textContent = '';
                            
                        }
                    }
                }

                if (element.type === 'email') {
                    if (!this.isValidEmail(element.value)) {
                        element.classList.add('is-invalid');
                        element.classList.remove('is-valid');
                        isValid = false;

                        // Display error message for <span> element with id="emailError"
                        const errorSpanId = 'emailError';
                        const errorSpan = document.getElementById(errorSpanId);
                        if (errorSpan) {
                            errorSpan.textContent = 'Invalid email address';
                            errorSpan.classList.add('invalid-feedback');
                            errorSpan.classList.remove('valid-feedback');
                        }
                    } else {
                        element.classList.add('is-valid');
                        element.classList.remove('is-invalid');

                        // Clear error message for <span> element with id="emailError"
                        const errorSpanId = 'emailError';
                        const errorSpan = document.getElementById(errorSpanId);
                        if (errorSpan) {
                            errorSpan.classList.add('valid-feedback');
                            errorSpan.classList.remove('invalid-feedback');
                            errorSpan.textContent = '';

                        }
                    }
                }

            });



        }



        return isValid;
    }

    isValidEmail(email) {

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }


    isValidNigeriaMobileNumber(number) {
        let dialingCode, mobilePrefix, firstFigure, checkArray;

        let telcoPrefixes = [703, 706, 803, 806, 810, 813, 814, 816, 903, 705, 805, 811, 815, 905, 701, 708, 802, 808, 812, 902, 809, 817, 818, 909, 908, 804,901,911,912,907,904,902, 807,811, 915,906, 913, 916];

       const phoneInputValue = number;

        var inputLength = phoneInputValue.length;

        if (inputLength < 11) {
            return false;

        } else if (inputLength === 11) {

            mobilePrefix = Number(phoneInputValue.substr(1, 3));
            firstFigure = Number(phoneInputValue[0]);

            checkArray = in_array(mobilePrefix, telcoPrefixes);
            if (checkArray === false) {
                return false;
            } else if (checkArray > 0 && firstFigure === 0) {
                return true;
            } else {
                return false;
            }
        } else if (inputLength === 13) {
            mobilePrefix = Number(phoneInputValue.substr(3, 3));
            dialingCode = Number(phoneInputValue.substr(0, 3));
            checkArray = in_array(mobilePrefix, telcoPrefixes);
            if (checkArray === false) {
                return false;
            } else if ((checkArray >= 0) && (dialingCode === 234)) {
                return true;
            } else {
                return false;
            }
        } else if (inputLength === 14) {
            mobilePrefix = Number(phoneInputValue.slice(4, 7));
            dialingCode = phoneInputValue.slice(0, 4);
            checkArray = in_array(mobilePrefix, telcoPrefixes);
            if (checkArray === false) {

                return false;
            } else if ((checkArray >= 0) && (dialingCode === "+234")) {
                return true;
            } else {
                return false;
            }
        } else if (inputLength > 14) {
            return false;
        }
        return false;
    }

}

function in_array(value, array) {
    let index = array.indexOf(value);
    if (index == -1) {
        return false;
    } else {
        return index;
    }
}

