// Segurança para trafegar a senha com Hash
async function hashSenha(plainText) {
  const encoder = new TextEncoder();
  const data = encoder.encode(plainText);
  const hashBuffer = await crypto.subtle.digest('SHA-256', data);
  const hashArray = Array.from(new Uint8Array(hashBuffer));
  const hashHex = hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
  return hashHex;
}

// Temporizador
function debounce(func, delay) {
  let timer;
  return function (...args) {
    clearTimeout(timer);
    timer = setTimeout(() => func.apply(this, args), delay);
  };
}

// Formata Data/Hora para o padrão brasileiro
function formatPtBrDateTime(datatime)
{
  return `${new Date(datatime).toLocaleDateString('pt-BR')} ${new Date(datatime).toLocaleTimeString('pt-BR', { hour12: false })}`;
}

// Obtém os dados dos formulários
function getFormData(form) {
  const unindexed_array = form.serializeArray();
  const indexed_array = {};

  $.map(unindexed_array, function (n) {
    indexed_array[n['name']] = n['value'];
  });

  return indexed_array;
}

/* Personalização global do ToastR (Notificações) */
toastr.options = {
  "closeButton": true,
  "debug": false,
  "newestOnTop": true,
  "progressBar": true,
  "positionClass": "toast-top-right",
  "preventDuplicates": true,
  "showDuration": "300",
  "hideDuration": "500",
  "timeOut": "2000",
  "extendedTimeOut": "1000",
  "showEasing": "swing",
  "hideEasing": "linear",
  "showMethod": "fadeIn",
  "hideMethod": "fadeOut"
};