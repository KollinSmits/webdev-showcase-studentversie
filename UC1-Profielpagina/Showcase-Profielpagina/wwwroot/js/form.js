const inputEmail = document.getElementById('email');
const inputFirstName = document.getElementById('firstname');
const inputLastName = document.getElementById('lastname');
const inputPhone = document.getElementById('phone');
const inputSubject = document.getElementById('subject');
const inputMessage = document.getElementById('message');


const validateEmail = () => {
    const emailRegex = /^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$/;

    if (!emailRegex.test(inputEmail.value)) {
        inputEmail.setCustomValidity("Voer een geldig e-mailadres in!");
    } else if (inputEmail.value.length > 30) {
        inputEmail.setCustomValidity("Email moet niet langer dan 30 tekens zijn!");
    } else {
        inputEmail.setCustomValidity("");
    }
};

const validateFirstName = () => {
    if (inputFirstName.value.trim() === "") {
        inputFirstName.setCustomValidity("Voornaam is verplicht.");
        inputFirstName.reportValidity();
        return false;
    } else if (inputFirstName.value.length > 30) {
        inputFirstName.setCustomValidity("Voornaam mag niet langer dan 30 tekens zijn.");
        inputFirstName.reportValidity();
        return false;
    } else {
        inputFirstName.setCustomValidity("");
        return true;
    }
};

const validateLastName = () => {
    if (inputLastName.value.trim() === "") {
        inputLastName.setCustomValidity("Achternaam is verplicht.");
        inputLastName.reportValidity();
        return false;
    } else if (inputLastName.value.length > 30) {
        inputLastName.setCustomValidity("Achternaam mag niet langer dan 30 tekens zijn.");
        inputLastName.reportValidity();
        return false;
    } else {
        inputLastName.setCustomValidity("");
        return true;
    }
};

const validatePhone = () => {
    const phoneRegex = /^[0-9]{8,15}$/;
    if (!phoneRegex.test(inputPhone.value)) {
        inputPhone.setCustomValidity("Voer een geldig telefoonnummer in (8-15 cijfers).");
    } else {
        inputPhone.setCustomValidity("");
    }
    inputPhone.reportValidity();
};

const validateSubject = () => {
    if (inputSubject.value.length > 30) {
        inputSubject.setCustomValidity("Onderwerp mag niet langer dan 30 tekens zijn.");
    } else {
        inputSubject.setCustomValidity("");
    }
    inputSubject.reportValidity();
};

const validateMessage = () => {
    if (inputMessage.value.length < 20 || inputMessage.value.length > 600) {
        inputMessage.setCustomValidity("Bericht moet tussen de 20 en 600 tekens zijn.");
    } else {
        inputMessage.setCustomValidity("");
    }
    inputMessage.reportValidity();
};


inputEmail.addEventListener("blur", validateEmail);
inputEmail.addEventListener("input", validateEmail);

inputSubject.addEventListener("blur", validateSubject);
inputSubject.addEventListener("input", validateSubject);

inputFirstName.addEventListener("blur", validateFirstName);
inputFirstName.addEventListener("input", validateFirstName);

inputLastName.addEventListener("blur", validateLastName);
inputLastName.addEventListener("input", validateLastName);

inputPhone.addEventListener("blur", validatePhone);
inputPhone.addEventListener("input", validatePhone);

inputMessage.addEventListener("blur", validateMessage);
inputMessage.addEventListener("input", validateMessage);

const form = document.getElementById('contactform');

const createMessageContainer = () => {
    let messageContainer;
    if (!messageContainer) {
        messageContainer = document.createElement("div");
        messageContainer.id = "messageContainer";
        messageContainer.style.position = "fixed";
        messageContainer.style.top = "10px";
        messageContainer.style.left = "50%";
        messageContainer.style.transform = "translateX(-50%)";
        messageContainer.style.padding = "10px";
        messageContainer.style.border = "3px solid #ccc";
        messageContainer.style.borderRadius = "5px";
        messageContainer.style.color = "#ffffff";
        messageContainer.style.display = "none";
        document.body.appendChild(messageContainer);
    }
    return messageContainer;
};

const showMessage = (message, isSuccess) => {
    const messageContainer = createMessageContainer();
    messageContainer.textContent = message;
    messageContainer.style.backgroundColor = isSuccess ? "green" : "red";
    messageContainer.style.display = "block";
    document.getElementById("contactsection").appendChild(messageContainer);

    setTimeout(() => {
        messageContainer.style.display = "none";
    }, 3000);
};
let span = document.createElement('span');
function showLoader() {
    span.className = 'loader'
    document.getElementById('contactform').appendChild(span);
}

function hideLoader() {
    document.getElementsByName('submitButton').disabled = false;
    if (span) {
        span.remove();
    }
}

form.addEventListener('submit', async function (event) {
    event.preventDefault();

    document.getElementById('submitButton').disabled = true;
    showLoader();

    grecaptcha.ready(function () {
        grecaptcha.execute("6Le-OdkqAAAAAK3yUEvUqK7R-LEVFQrvxCiJitLl", { action: 'submit' }).then(async function (token) {
            const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]').value;
            const formData = new URLSearchParams();
            formData.append('email', inputEmail.value);
            formData.append('firstname', inputFirstName.value);
            formData.append('lastname', inputLastName.value);
            formData.append('phone', inputPhone.value);
            formData.append('subject', inputSubject.value);
            formData.append('message', inputMessage.value);
            formData.append('__RequestVerificationToken', csrfToken);
            formData.append('Recaptcha', token);
            console.log(token);
            try {
                form.reset();

                const response = await fetch('/contact', {
                    method: 'POST',
                    //headers: {
                    //    'Content-Type': 'application/x-www-form-urlencoded'
                    //},
                    body: formData
                });

                if (!response.ok) {
                    throw new Error('Netwerkrespons was not ok');
                }
                hideLoader();
                showMessage("the form is submitted!", true);
                console.log("the form is submitted!");

            } catch (error) {
                form.reset();
                console.error('there was a problem with the submited form:', error);
                hideLoader();
                showMessage("the form was not submitted", false);

            }
        });
    });
});