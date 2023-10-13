
//validacao campo data de nascimento - vol
const form = $("#myForm");
const dataNascimentoInput = $("#inpDataNasc");
const idadeMessage = $("#idadeMessage");

dataNascimentoInput.addEventListener("input", function () {
    const dataNascimento = new Date(this.value);
    const hoje = new Date();
    const idade = hoje.getFullYear() - dataNascimento.getFullYear();

    // Verifica se o mês de aniversário já ocorreu
    if (hoje.getMonth() < dataNascimento.getMonth() || (hoje.getMonth() === dataNascimento.getMonth() && hoje.getDate() < dataNascimento.getDate())) {
        idade--;
    }

    if (idade < 16) {
        idadeMessage.textContent = "É necessário ser maior de 16 anos.";
        form.classList.add("invalid");
    } else {
        idadeMessage.textContent = "";
        form.classList.remove("invalid");
    }
});

form.addEventListener("submit", function (event) {
    if (form.classList.contains("invalid")) {
        event.preventDefault();
    }
});

const cpfInput = document.getElementById("cpf");
const cpfValidationMessage = document.getElementById("cpfValidationMessage");

cpfInput.addEventListener("input", function () {
    const cpf = cpfInput.value.replace(/\D/g, ''); // Remove caracteres não numéricos

    if (cpf.length === 11 && isValidCPF(cpf)) {
        cpfValidationMessage.textContent = "";
    } else {
        cpfValidationMessage.textContent = "CPF inválido";
    }
});

function isValidCPF(cpf) {
    cpf = cpf.replace(/\D/g, ''); // Remove caracteres não numéricos

    if (cpf.length !== 11) {
        return false;
    }

    // Verifica se todos os dígitos são iguais, o que torna o CPF inválido
    if (/^(\d)\1+$/.test(cpf)) {
        return false;
    }

    let sum = 0;
    let remainder;

    for (let i = 1; i <= 9; i++) {
        sum = sum + parseInt(cpf.substring(i - 1, i)) * (11 - i);
    }

    remainder = (sum * 10) % 11;

    if ((remainder === 10) || (remainder === 11)) {
        remainder = 0;
    }

    if (remainder !== parseInt(cpf.substring(9, 10))) {
        return false;
    }

    sum = 0;

    for (let i = 1; i <= 10; i++) {
        sum = sum + parseInt(cpf.substring(i - 1, i)) * (12 - i);
    }

    remainder = (sum * 10) % 11;

    if ((remainder === 10) || (remainder === 11)) {
        remainder = 0;
    }

    if (remainder !== parseInt(cpf.substring(10, 11))) {
        return false;
    }

    return true;
}


//Validação para campo de email
const emailInput = document.getElementById("email");
const emailValidationMessage = document.getElementById("emailValidationMessage");

emailInput.addEventListener("input", function () {
    const email = emailInput.value;

    if (isValidEmail(email)) {
        emailValidationMessage.textContent = "";
    } else {
        emailValidationMessage.textContent = "Email inválido";
    }
});

function isValidEmail(email) {
    return email.includes("@");
}

function validarSenha() {
    var senhaInput = document.getElementById("senha");
    var mensagemSenha = document.getElementById("mensagemSenha");

    var senha = senhaInput.value;

    // Adicione aqui suas regras de validação da senha
    if (senha.length < 8) {
        mensagemSenha.textContent = "A senha deve ter pelo menos 8 caracteres.";
    } else {
        mensagemSenha.textContent = ""; // Limpa a mensagem de erro se a senha for válida
    }
}


//Botao de avançar da tab
$("#avancarBtn").click(function () {
    // Verifique se os campos estão preenchidos
    var email = $("#email").val();
    var senha = $("#senha").val();
    var confirmarSenha = $("#confirmarSenha").val();

    if (!email || !senha || !confirmarSenha) {
        alert("Por favor, preencha todos os campos.");
        return;
    }

    // Verifique se a senha e a confirmação de senha correspondem
    if (senha !== confirmarSenha) {
        alert("A senha e a confirmação de senha não coincidem.");
        return;
    }

    // Remova a classe "active" da aba atual (home) e adicione-a à próxima aba (profile)
    $("#home").removeClass("active");
    $("#profile").addClass("active");
});


