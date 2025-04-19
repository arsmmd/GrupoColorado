function obterUsuarios() {
  $.ajax({
    url: '/Usuarios/GetAll',
    type: 'GET',
    success: function (result) {
      const select = $('#usuarioInsercao');
      result.data.forEach(function (item) {
        select.append(`<option value="${item.codigoUsuario}">${item.nome}</option>`);
      });
    },
    error: function () {
      toastr.error('Erro ao carregar os usuários.');
    }
  });
}

$(document).ready(function () {
  obterUsuarios();

  // Listagem no Data Table
  var table = $('#tiposTelefoneTable').DataTable({
    orderCellsTop: true,
    fixedHeader: true,
    searching: true,
    responsive: true,
    serverSide: true,
    processing: true,
    ajax: {
      url: '/TiposTelefone/Read',
      type: 'POST',
      dataSrc: function (json) {
        return json.data;
      },
      dataFilter: function (data) {
        let json = JSON.parse(data);
        return JSON.stringify({
          recordsTotal: json.count,
          recordsFiltered: json.count,
          data: json.data
        });
      }
    },
    columns: [
      { data: 'codigoTipoTelefone' },
      { data: 'descricaoTipoTelefone' },
      {
        data: 'dataInsercao',
        render: function (data) {
          return formatPtBrDateTime(data);
        }
      },
      {
        data: null,
        orderable: false,
        render: function (data, type, row) {
          return `
                                       <button class="btn btn-sm btn-primary btn-update" data-codigotipotelefone="${row.codigoTipoTelefone}" title="Editar">
                                         <i class="fas fa-edit"></i>
                                       </button>
                                       <button class="btn btn-sm btn-danger btn-delete" data-codigotipotelefone="${row.codigoTipoTelefone}" title="Excluir">
                                         <i class="fas fa-trash-alt"></i>
                                       </button>
                                     `;
        }
      }
    ],
    language: {
      url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json"
    },
  });

  // Filtro com temporizador
  $('#tiposTelefoneTable thead tr:eq(1) th').each(function (i) {
    $('input', this).on('keyup change', debounce(function () {
      if (table.column(i).search() !== this.value) {
        table
          .column(i)
          .search(this.value)
          .draw();
      }
    }, 500));
  });


  // Inclusão
  $('#btnAdicionar').on('click', function () {
    $('#codigoTipoTelefone').val(0);
    $('#descricaoTipoTelefone').val("");
    $('#dataInsercao').val("");
    $('#usuarioInsercao').val("");
    $('.readonly-fields').hide();

    $("#modalTipoTelefoneLabel").html("Cadastrar Tipo de Telefone");
    $('#modalTipoTelefone').modal('show');
  });


  // Exclusão
  $(document).on('click', '.btn-delete', function () {
    const key = $(this).data('codigotipotelefone');
    Swal.fire({
      title: 'Confirma a exclusão do registro?',
      text: "Essa operação é irreversível",
      icon: 'warning',
      showCancelButton: true
    }).then((result) => {
      if ((result.isConfirmed) && (key !== null)) {
        $.ajax({
          url: `/TiposTelefone/Delete?codigoTipoTelefone=${key}`,
          type: 'DELETE',
          success: function () {
            toastr.success('Registro excluído com sucesso!');
            $('#tiposTelefoneTable').DataTable().ajax.reload(null, false);
          },
          error: function (e) {
            toastr.error(e.responseText);
          }
        });
      }
    });
  });


  // Popup para atualização
  $(document).on('click', '.btn-update', function () {
    const key = $(this).data('codigotipotelefone');

    $.get(`/TiposTelefone/GetByPk?codigoTipoTelefone=${key}`, function (result) {
      if (!(result.success)) {
        toastr.error("Não foi possível obter os dados.");
        return;
      }

      const tipoTelefone = result.data;
      $('#codigoTipoTelefone').val(tipoTelefone.codigoTipoTelefone);
      $('#descricaoTipoTelefone').val(tipoTelefone.descricaoTipoTelefone);
      $('#dataInsercao').val(formatPtBrDateTime(tipoTelefone.dataInsercao));
      $('#usuarioInsercao').val(tipoTelefone.usuarioInsercao);
      $('.readonly-fields').show();

      $("#modalTipoTelefoneLabel").html("Editar Tipo de Telefone");
      $('#modalTipoTelefone').modal('show');
    });
  });


  // Submit do Form
  $('#formTipoTelefone').on('submit', function (e) {
    e.preventDefault();

    const $btn = $('#btnSalvar');
    $btn.prop('disabled', true).text('Salvando...');

    let formData = getFormData($(this));
    // Remove os 2 campos que não serão necessários, já que quem define esses valores é a API.
    delete formData.dataInsercao;
    delete formData.usuarioInsercao;

    let endpoint;
    let type;
    let message;
    if (formData.codigoTipoTelefone == 0) {
      endpoint = 'Create';
      type = 'POST';
      message = 'inserido';
    }
    else {
      endpoint = 'Update';
      type = 'PUT';
      message = 'atualizado';
    }

    $.ajax({
      url: `/TiposTelefone/${endpoint}`,
      type,
      contentType: 'application/json',
      data: JSON.stringify(formData),
      success: function () {
        $('#tiposTelefoneTable').DataTable().ajax.reload(null, false);
        $('#modalTipoTelefone').modal('hide');
        toastr.success(`Registro ${message} com sucesso!`);
        $btn.prop('disabled', false).text('Salvar');
      },
      error: function (e) {
        toastr.error(e.responseText);
        $btn.prop('disabled', false).text('Salvar');
      }
    });
  });
});