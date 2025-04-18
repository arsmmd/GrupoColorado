﻿$(document).ready(function () {

  // Listagem no Data Table
  var table = $('#usuariosTable').DataTable({
    orderCellsTop: true,
    fixedHeader: true,
    searching: true,
    responsive: true,
    serverSide: true,
    processing: true,
    ajax: {
      url: '/Clientes/Read',
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
      { data: 'codigoCliente' },
      { data: 'razaoSocial' },
      { data: 'tipoPessoa' },
      { data: 'documento' },
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
                                             <a href="/Clientes/Details?codigoCliente=${row.codigoCliente}" class="btn btn-sm btn-primary me-1" title="Editar">
                                               <i class="fas fa-edit"></i>
                                             </a>
                                             <button class="btn btn-sm btn-danger btn-delete" data-codigousuario="${row.codigoCliente}" title="Excluir">
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
          url: `/Clientes/Delete?codigoCliente=${key}`,
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
});