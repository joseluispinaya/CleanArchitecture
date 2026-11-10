
let tablaData;
let idEditar = 0;
const controlador = "UnidadEd";

$(function () {

    // 2. Inicializar DataTables
    tablaData = $('#tbData').DataTable({
        responsive: true,
        ajax: {
            url: `/${controlador}/ListaFullUnidades`,
            type: "GET",
            datatype: "json",
            // IMPORTANTE: Le decimos a DataTables que nuestra lista está dentro de la propiedad "data"
            dataSrc: "data"
        },
        columns: [
            { data: "idUnidadEducativa", visible: false, searchable: false },
            {
                title: "Unidad Educativa",
                data: null,
                searchable: false,
                render: function (data, type, row) {
                    return `
                        <div class="d-flex align-items-center">
                            <div class="w-40px h-40px rounded-circle d-flex align-items-center justify-content-center bg-light me-3">
                                <i class="fas fa-school"></i>
                            </div>
                            <div>
                                <div class="fw-bold fs-14px text-body">${row.nombre}</div>
                                <div class="fs-12px">Resp: ${row.responsable}</div>
                            </div>
                        </div>
                    `;
                }
            },
            {
                title: "Información",
                data: null,
                className: "text-start align-middle",
                render: function (data, type, row) {
                    return `<div class="fw-bold text-dark"><i class="fas fa-phone text-success me-1"></i>${row.nroContacto}</div>
                            <div class="fs-12px text-muted"><i class="far fa-calendar-alt me-1"></i>${row.fechaCreadoStr}</div>`;
                }
            },
            {
                title: "Acción",
                data: "idUnidadEducativa",
                orderable: false,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-primary btn-sm btn-editar" data-id="${data}" title="Editar">
                            <i class="fas fa-pencil-alt"></i>
                        </button>
                        <button class="btn btn-danger btn-sm btn-estado ms-1" data-id="${data}" title="Estado">
                            <i class="fas fa-flag"></i>
                        </button>
                    `;
                }
            }
        ],
        order: [[0, "desc"]],
        language: {
            url: "https://cdn.datatables.net/plug-ins/3.1.5/i18n/es-ES.json"
        }
    });

});

$("#btnRegistro").on("click", function () {
    idEditar = 0;

    // Limpia todos los inputs que tengan la clase 'model'
    $(".model").val("");
    $("#myModalLabel").text("Nuevo Registro");
    $("#mdData").modal("show");
});

// fin