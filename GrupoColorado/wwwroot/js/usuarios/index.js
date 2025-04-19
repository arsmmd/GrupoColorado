$(document).ready(function () {

  // Listagem no Data Table
  var table = $('#usuariosTable').DataTable({
    orderCellsTop: true,
    fixedHeader: true,
    searching: true,
    responsive: true,
    serverSide: true,
    processing: true,
    ajax: {
      url: '/Usuarios/Read',
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
      { data: 'codigoUsuario' },
      { data: 'nome' },
      { data: 'email' },
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
                                     <button class="btn btn-sm btn-primary btn-update" data-codigousuario="${row.codigoUsuario}" title="Editar">
                                       <i class="fas fa-edit"></i>
                                     </button>
                                     <button class="btn btn-sm btn-danger btn-delete" data-codigousuario="${row.codigoUsuario}" title="Excluir">
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
  $('#usuariosTable thead tr:eq(1) th').each(function (i) {
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
    $('#codigoUsuario').val(0);
    $('#nome').val("");
    $('#email').val("");
    $('#senha').val("");
    $('#ativo').prop('checked', true);
    $('#dataInsercao').val("");
    $('.readonly-fields').hide();
    $('.writeonly-fields').show();

    $("#modalUsuarioLabel").html("Cadastrar Usuário");
    $('#modalUsuario').modal('show');
  });


  // Exclusão
  $(document).on('click', '.btn-delete', function () {
    const key = $(this).data('codigousuario');
    Swal.fire({
      title: 'Confirma a exclusão do registro?',
      text: "Essa operação é irreversível",
      icon: 'warning',
      showCancelButton: true
    }).then((result) => {
      if ((result.isConfirmed) && (key !== null)) {
        $.ajax({
          url: `/Usuarios/Delete?codigoUsuario=${key}`,
          type: 'DELETE',
          success: function () {
            toastr.success('Registro excluído com sucesso!');
            $('#usuariosTable').DataTable().ajax.reload(null, false);
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
    const key = $(this).data('codigousuario');

    $.get(`/Usuarios/GetByPk?codigoUsuario=${key}`, function (result) {
      if (!(result.success)) {
        toastr.error("Não foi possível obter os dados.");
        return;
      }

      const usuario = result.data;
      $('#codigoUsuario').val(usuario.codigoUsuario);
      $('#nome').val(usuario.nome);
      $('#email').val(usuario.email);
      $('#senha').val("------");
      $('#ativo').prop('checked', usuario.ativo);
      $('#dataInsercao').val(formatPtBrDateTime(usuario.dataInsercao));
      $('.readonly-fields').show();
      $('.writeonly-fields').hide();

      $("#modalUsuarioLabel").html("Editar Usuário");
      $('#modalUsuario').modal('show');
    });
  });


  // Submit do Form
  $('#formUsuario').on('submit', async function (e) {
    e.preventDefault();

    const $btn = $('#btnSalvar');
    $btn.prop('disabled', true).text('Salvando...');

    let formData = getFormData($(this));
    formData.ativo = $('#ativo').prop('checked');
    // Remove o campo que não é necessário, já que quem define esse valor é a API.
    delete formData.dataInsercao;

    // Criando o Hash da senha
    formData.senha = await hashSenha(formData.senha);

    let endpoint;
    let type;
    let message;
    if (formData.codigoUsuario == 0) {
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
      url: `/Usuarios/${endpoint}`,
      type,
      contentType: 'application/json',
      data: JSON.stringify(formData),
      success: function () {
        $('#usuariosTable').DataTable().ajax.reload(null, false);
        $('#modalUsuario').modal('hide');
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