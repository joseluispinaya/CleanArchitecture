
let tablaData;
let idEditar = 0;
const controlador = "Directores";

$(function () {

    // 2. Inicializar DataTables
    tablaData = $('#tbData').DataTable({
        responsive: true,
        ajax: {
            url: `/${controlador}/ListaDirectoresConvert`,
            type: "GET",
            datatype: "json",
            // IMPORTANTE: Le decimos a DataTables que nuestra lista está dentro de la propiedad "data"
            dataSrc: "data"
        },
        columns: [
            { data: "idDirector", visible: false, searchable: false },
            {
                title: "Director(a)",
                data: null,
                searchable: false,
                render: function (data, type, row) {
                    let logo = row.Photo ? row.Photo : '/imagen/sinimagen.png';
                    return `
                        <div class="d-flex align-items-center">
                            <div class="w-40px h-40px rounded-circle d-flex align-items-center justify-content-center bg-light overflow-hidden shadow-sm me-3">
                                <img src="${logo}" alt="" class="mw-100 mh-100" />
                            </div>
                            <div>
                                <div class="fw-bold fs-14px text-body">${row.fullName}</div>
                                <div class="fs-12px text-gray-500">U.E: ${row.nombreUnidadEducativa}</div>
                            </div>
                        </div>
                    `;
                }
            },
            {
                title: "Contacto (Correo)",
                data: null,
                className: "text-start align-middle",
                render: function (data, type, row) {
                    return `<div class="fw-bold text-dark">${row.nroCi}</div>
                            <div class="fs-12px text-muted"><i class="fa fa-envelope me-1"></i>${row.correo}</div>`;
                }
            },
            {
                title: "Estado",
                data: "estado",
                render: function (data) {
                    return data
                        ? `<span
                                class="badge border border-warning text-warning px-2 pt-5px pb-5px rounded fs-12px d-inline-flex align-items-center">
                                <i class="fa fa-check fs-9px fa-fw me-5px"></i>Activo
                            </span>`
                        : `<span
                                class="badge border border-danger text-danger px-2 pt-5px pb-5px rounded fs-12px d-inline-flex align-items-center">
                                <i class="fa fa-times fs-9px fa-fw me-5px"></i>Inactivo
                            </span>`;
                }
            },
            {
                title: "Acción",
                data: "idDirector",
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

$("#btnNuevoUsuario").on("click", function () {
    idEditar = 0;

    // Limpia todos los inputs que tengan la clase 'model'
    $(".model").val("");
    $('#file3Remove').addClass('d-none');

    $("#previewImg3").attr("src", "/imagen/sinimagen.png");

    $("#myModalLabel").text("Registrar Director");
    $("#mdUsuario").modal("show");
});

function esImagen(file) {
    return file && file.type.startsWith("image/");
}

function mostrarImagenSeleccionadaP(input) {
    let file = input.files && input.files[0];


    if (file) {
        let reader = new FileReader();
        if (!esImagen(file)) {
            MensajeToast("Error", "El archivo seleccionado no es una imagen válida.", "error");
            resetearVistaFoto(input);
            return;
        }


        reader.onload = function (e) {
            $('#previewImg3').attr('src', e.target.result);
            $('#file3Remove').removeClass('d-none');
        }
        reader.readAsDataURL(file);
    } else {
        // Si NO se seleccionó archivo (ej: presionaron "Cancelar")
        $('#previewImg3').attr('src', "/imagen/sinimagen.png");
    }
}

$("#fileInput3").on("change", function () {
    mostrarImagenSeleccionadaP(this);
});

function resetearVistaFoto(input) {
    $('#previewImg3').attr('src', "/imagen/sinimagen.png");
    input.value = "";
    $('#file3Remove').addClass('d-none');
}

$("#file3Remove").on("click", function () {
    resetearVistaFoto(document.getElementById('fileInput3'));
});

function habilitarBoton() {
    $('#btnGuardarCambios').prop('disabled', false);
}

$("#btnGuardarCambios").on("click", function () {

    $('#btnGuardarCambios').prop('disabled', true);

    const inputs = $("input.model").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() === "");

    if (inputs_sin_valor.length > 0) {
        const nombreCampo = inputs_sin_valor[0].name;
        const mensaje = `Debe completar el campo: "${nombreCampo}"`;
        MensajeToast("Advertencia", mensaje, "warning");

        // El selector ahora funcionará correctamente porque quitamos el espacio del 'name' en el HTML
        $(`input[name="${nombreCampo}"]`).focus();

        habilitarBoton();
        return;
    }

    const modelo = {
        IdDirector: idEditar,
        //IdUnidadEducativa: parseInt($("#cboUnidadEd").val()),
        Nombres: $("#txtNombres").val().trim(),
        Apellidos: $("#txtApellidos").val().trim(),
        NroCi: $("#txtNroci").val().trim(),
        Celular: $("#txtCelular").val().trim(),
        Correo: $("#txtCorreo").val().trim(),
        Estado: ($("#cboEstado").val() === "1" ? true : false)
    };

    const fileInput = document.getElementById('fileInput3');

    const formData = new FormData();

    if (fileInput.files.length > 0) {
        formData.append("foto", fileInput.files[0]);
    }

    formData.append("modelo", JSON.stringify(modelo));

    // --- CÓDIGO PARA IMPRIMIR EN CONSOLA ---
    console.log("--- CONTENIDO DEL FORMDATA ---");
    for (const [clave, valor] of formData.entries()) {
        if (clave === "modelo") {
            // Convierte el string JSON de vuelta a objeto para verlo ordenado en la consola
            console.log(`${clave}:`, JSON.parse(valor));
        } else if (valor instanceof File) {
            // Muestra detalles del archivo (nombre, tamaño, tipo)
            console.log(`${clave} (Archivo):`, {
                nombre: valor.name,
                tamaño: `${(valor.size / 1024).toFixed(2)} KB`,
                tipo: valor.type
            });
        } else {
            console.log(`${clave}:`, valor);
        }
    }
    // ----------------------------------------
    habilitarBoton();

});

function obtenerDirectorConvertido() {
    fetch(`/${controlador}/ObtenerDirectorConverter?idDirector=1`, {
        method: "PUT"
    })
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            // Muestra el objeto JSON completo devuelto por tu controlador
            console.log("Director convertido:", responseJson);

            // Si quieres ver específicamente la lista de datos:
            // console.log("Lista de Tipos:", responseJson.data);
        })
        .catch((error) => {
            console.error("Error en la petición fetch:", error);
        });
}


// fin