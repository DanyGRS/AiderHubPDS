const inputCep = document.getElementById("cep");
const inputLogradouro = document.getElementById("logradouro");
const inputNumero = document.getElementById("numero");
const inputComplemento = document.getElementById("complemento");
const inputBairro = document.getElementById("bairro");
const inputCidade = document.getElementById("cidade");
const inputUf = document.getElementById("uf");

inputCep.addEventListener("blur", () => {
    const cep = inputCep.value.replace("-", "");
    const url = `https://viacep.com.br/ws/${cep}/json/`;

    fetch(url)
        .then((response) => response.json())
        .then((data) => {
            inputLogradouro.value = data.logradouro;
            inputComplemento.value = data.complemento;
            inputBairro.value = data.bairro;
            inputCidade.value = data.localidade;
            inputUf.value = data.uf;
            inputNumero.focus();
        })
        .catch((error) => {
            console.log(error);
        });
});
