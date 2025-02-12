const inputEmail = document.getElementById('email');
const inputFirstName = document.getElementById('firstname');
const inputLastName = document.getElementById('lastname');
const inputPhone = document.getElementById('phone');
const inputSubject = document.getElementById('subject');
const inputMessage = document.getElementById('message');


const validateEmail = () => {
    if (inputEmail.validity.typeMismatch) {
        inputEmail.setCustomValidity("Voer een geldig e-mailadres in!");
        inputEmail.reportValidity();
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
    } else if (inputFirstName.value.length > 60) {
        inputFirstName.setCustomValidity("Voornaam mag niet langer dan 60 tekens zijn.");
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
    } else if (inputLastName.value.length > 60) {
        inputLastName.setCustomValidity("Achternaam mag niet langer dan 60 tekens zijn.");
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
    if (inputMessage.value.length < 20 || inputMessage.value.length > 200) {
        inputMessage.setCustomValidity("Bericht moet tussen 20 en 200 tekens zijn.");
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
form.addEventListener('submit', async function (event) {
    event.preventDefault();

    //document.getElementById('test1').innerHTML = '<span class="loader"></span>';

    //let span = document.createElement('span');
    //span.className = 'loader'
    //document.body.appendChild(span);
    //document.getElementById('test1').appendChild(span);

    const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

    const formData = new URLSearchParams();

    formData.append('email', inputEmail.value);
    formData.append('firstname', inputFirstName.value);
    formData.append('lastname', inputLastName.value);
    formData.append('phone', inputPhone.value);
    formData.append('subject', inputSubject.value);
    formData.append('message', inputMessage.value);
    formData.append('__RequestVerificationToken', csrfToken);

    try {

        const response = await fetch('/contact', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: formData
        });

        if (!response.ok) {
            throw new Error('Netwerkrespons was niet ok');
        }
        form.reset();
        console.log("het formulier is verzonden!");

    } catch (error) {
        console.error('Er was een probleem met de formulierinzending:', error);
        form.reset();


    }


});