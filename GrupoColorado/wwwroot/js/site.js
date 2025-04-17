function debounce(func, delay) {
  let timer;
  return function (...args) {
    clearTimeout(timer);
    timer = setTimeout(() => func.apply(this, args), delay);
  };
}

function formatPtBrDateTime(datatime)
{
  return `${new Date(datatime).toLocaleDateString('pt-BR')} ${new Date(datatime).toLocaleTimeString('pt-BR', { hour12: false })}`;
}

function getFormData(form) {
  var unindexed_array = form.serializeArray();
  var indexed_array = {};

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