document.querySelector("form").addEventListener("submit", function (event) {
    event.preventDefault();  
    let formData = new FormData(event.target);
    console.log(Object.fromEntries(formData.entries())); // Laat de data zien
});

const inputEmail = document.getElementById('email');
const inputFirstName = document.getElementById('firstname');
const inputLastName = document.getElementById('lastname');
const inputPhone = document.getElementById('phone');
const inputSubject = document.getElementById('subject');
const inputMessage = document.getElementById('message');

// validatie voor email 
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

// validatie voor voor en achternaam
const validateName = (input) => {
    const nameRegex = /^[a-zA-Z]+$/;
    if (!nameRegex.test(input.value)) {
        input.setCustomValidity("Mag alleen letters bevatten.");
    } else if (input.value.length > 20) {
        input.setCustomValidity("Mag niet langer dan 20 tekens zijn.");
    } else {
        input.setCustomValidity("");
    }
    input.reportValidity();
};

// Validatie voor Phonenumber (alleen cijfers, 8-15 tekens)
const validatePhone = () => {
    const phoneRegex = /^[0-9]{8,15}$/;
    if (!phoneRegex.test(inputPhone.value)) {
        inputPhone.setCustomValidity("Voer een geldig telefoonnummer in (8-15 cijfers).");
    } else {
        inputPhone.setCustomValidity("");
    }
    inputPhone.reportValidity();
};

// Validatie voor Subject (max. 30 tekens)
const validateSubject = () => {
    if (inputSubject.value.length > 30) {
        inputSubject.setCustomValidity("Onderwerp mag niet langer dan 30 tekens zijn.");
    } else {
        inputSubject.setCustomValidity("");
    }
    inputSubject.reportValidity();
};

// Validatie voor Message (20-200 tekens)
const validateMessage = () => {
    if (inputMessage.value.length < 20 || inputMessage.value.length > 200) {
        inputMessage.setCustomValidity("Bericht moet tussen 20 en 200 tekens zijn.");
    } else {
        inputMessage.setCustomValidity("");
    }
    inputMessage.reportValidity();
};

const validateForm = () => {
    validateEmail();
    validateMessage();
    validateName();
    validatePhone();
    validateSubject();
}

// Event listener voor email
// Aanbevolen events voor formulieren: https://github.com/Windesheim-HBO-ICT/client_studenten/blob/main/lessen/week-2/les-1/form-constraint-validation-api/studentversie/events-voor-invoer-validatie.md
inputEmail.addEventListener("blur", validateEmail);
inputEmail.addEventListener("input", validateEmail);
inputFirstName.addEventListener("input", () => validateName(inputFirstName));
inputLastName.addEventListener("input", () => validateName(inputLastName));
inputPhone.addEventListener("input", validatePhone);
inputSubject.addEventListener("input", validateSubject);
inputMessage.addEventListener("input", validateMessage);

// Selecteer het formelement
const form = document.querySelector('.form-contactpagina');

// Event listener voor formulierinzending
form.addEventListener('submit', function (event) {
    event.preventDefault(); // Voorkom standaard formulierinzending

    validateForm();

    // Verkrijg CSRF-token van het formulier
    const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

    // Serialiseer formuliergegevens
    const formData = new URLSearchParams();

    formData.append('email', form.email.value);
    formData.append('firstname', inputFirstName.value);
    formData.append('lastname', inputLastName.value);
    formData.append('phone', inputPhone.value);
    formData.append('subject', inputSubject.value);
    formData.append('message', inputMessage.value);
    formData.append('__RequestVerificationToken', csrfToken); // Voeg CSRF-token toe

    // Voer een POST-verzoek uit
    fetch('/contact', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json' // Stel de inhoudstype in
        },
        body: JSON.stringify(Object.fromEntries(formData))// Stuur de geserialiseerde formuliergegevens als de body
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Networkrespons was not ok');
                alert("network not responding")
            }
            return response.text();
        })
        .then(data => {
            // Verwerk succesvolle formulierinzending
            console.log('Formulier succesvol ingediend:', data);
            alert("Formulier succesvol verzonden!");
            return false;
            // Optioneel: je kunt hier een redirect uitvoeren of een succesbericht tonen
        })
        .catch(error => {
            console.error('Er was een probleem met de formulierinzending:', error);

            alert(error.message)

            // Verwerk fouten hier
        });
});