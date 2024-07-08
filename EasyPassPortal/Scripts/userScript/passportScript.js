$(document).ready(function () {
    $("#passportForm").validate({
        rules: {
            FullName: {
                required: true,
                minlength: 2
            },
            FatherName: {
                required: true,
                minlength: 2
            },
            MothersName: {
                required: true,
                minlength: 2
            },
            Gender: {
                required: true
            },
            DateOfBirth: {
                required: true,
                date: true
            },
            Address: {
                required: true,
                minlength: 5
            },
            Religion: {
                required: true
            },
            State: {
                required: true
            },
            District: {
                required: true
            },
            PhoneNumber: {
                required: true,
                digits: true,
                minlength: 10,
                maxlength: 10
            },
            Email: {
                required: true,
                email: true
            },
            AadharNumber: {
                required: true,
                digits: true,
                minlength: 12,
                maxlength: 12
            },
            PancardNumber: {
                required: true,
                minlength: 10,
                maxlength: 10
            },
            Education: {
                required: true
            },
            image1: {
                required: true,
                extension: "jpg|jpeg|png"
            },
            aadharPhoto1: {
                required: true,
                extension: "jpg|jpeg|png"
            },
            idProof1: {
                required: true,
                extension: "jpg|jpeg|png"
            },
            declarationCheckbox: {
                required: true
            }
        },
        messages: {
            FullName: {
                required: "Please enter your full name",
                minlength: "Full name must be at least 2 characters long"
            },
            FatherName: {
                required: "Please enter your father's name",
                minlength: "Father's name must be at least 2 characters long"
            },
            MothersName: {
                required: "Please enter your mother's name",
                minlength: "Mother's name must be at least 2 characters long"
            },
            Gender: {
                required: "Please select your gender"
            },
            DateOfBirth: {
                required: "Please enter your date of birth",
                date: "Please enter a valid date"
            },
            Address: {
                required: "Please enter your address",
                minlength: "Address must be at least 5 characters long"
            },
            Religion: {
                required: "Please enter your religion"
            },
            State: {
                required: "Please select your state"
            },
            District: {
                required: "Please select your district"
            },
            PhoneNumber: {
                required: "Please enter your phone number",
                digits: "Please enter a valid phone number",
                minlength: "Phone number must be 10 digits long",
                maxlength: "Phone number must be 10 digits long"
            },
            Email: {
                required: "Please enter your email",
                email: "Please enter a valid email address"
            },
            AadharNumber: {
                required: "Please enter your Aadhar number",
                digits: "Please enter a valid Aadhar number",
                minlength: "Aadhar number must be 12 digits long",
                maxlength: "Aadhar number must be 12 digits long"
            },
            PancardNumber: {
                required: "Please enter your PAN card number",
                minlength: "PAN card number must be 10 characters long",
                maxlength: "PAN card number must be 10 characters long"
            },
            Education: {
                required: "Please enter your education"
            },
            image1: {
                required: "Please upload your image",
                extension: "Please upload a valid image file (jpg, jpeg, png)"
            },
            aadharPhoto1: {
                required: "Please upload your Aadhar photo",
                extension: "Please upload a valid image file (jpg, jpeg, png)"
            },
            idProof1: {
                required: "Please upload your ID proof",
                extension: "Please upload a valid image file (jpg, jpeg, png)"
            },
            declarationCheckbox: {
                required: "You must agree to the declaration"
            }
        },
        errorElement: "div",
        errorPlacement: function (error, element) {
            error.addClass("text-danger");
            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass("is-invalid").removeClass("is-valid");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass("is-invalid").addClass("is-valid");
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
});