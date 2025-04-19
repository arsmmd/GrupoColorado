$(document).ready(function () {
  $('#formLogin').on('submit', async function (e) {
    e.preventDefault();

    const $btn = $('#btnLogin');
    $btn.prop('disabled', true).text('Entrando...');

    const formData = {
      email: $('#Email').val(),
      password: await hashSenha($('#Password').val()) // Criando o Hash da senha
    };

    $.ajax({
      url: '/Account/Login',
      type: 'POST',
      contentType: 'application/json',
      data: JSON.stringify(formData),
      success: function (res) {
        window.location.href = '/';
      },
      error: function (xhr) {
        toastr.error(xhr.responseText || 'Falha ao realizar login.');
        $btn.prop('disabled', false).text('Entrar');
      }
    });
  });
});