
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
                    let logo = row.photo ? row.photo : '/imagen/sinimagen.png';
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

function cargarUndidadesEd(idUniEd, idSeleccionar) {

    $("#cboUnidadEd").html('<option value="">Cargando unds educativas...</option>');
    $("#cboUnidadEd").prop("disabled", true);

    fetch(`/UnidadEd/ComboLista?idUniEd=${idUniEd}`, {
        method: "GET",
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        }
    })
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            console.log("Datos del servidor:", responseJson);

            if (responseJson.estado) {
                const lista = responseJson.data;

                if (lista != null && lista.length > 0) {

                    let opcionesHTML = '<option value="">-- Seleccione Und Educativa --</option>';

                    $.each(lista, function (i, row) {
                        opcionesHTML += `<option value="${row.idUnidadEducativa}">${row.nombre}</option>`;
                    });

                    $("#cboUnidadEd").html(opcionesHTML);
                    $("#cboUnidadEd").prop("disabled", false);

                    // Seleccionar el elemento si se proporcionó un ID
                    if (idSeleccionar != null && idSeleccionar != "") {
                        $("#cboUnidadEd").val(idSeleccionar);
                    } else {
                        $("#cboUnidadEd").val("");
                    }
                } else {
                    $("#cboUnidadEd").html('<option value="">Sin unds educativas disponibles</option>');
                    $("#cboUnidadEd").prop("disabled", true);
                }

            } else {
                $("#cboUnidadEd").html('<option value="">Error al cargar</option>');
                $("#cboUnidadEd").prop("disabled", true);
                console.error("Error del servidor:", responseJson.mensaje);
            }

        })
        .catch(error => {
            console.error("Error en la petición fetch:", error);
            $("#cboUnidadEd").html('<option value="">Error de conexión</option>');
            $("#cboUnidadEd").prop("disabled", true);
        });
}

$('#tbData tbody').on('click', '.btn-editar', function () {

    let fila = $(this).closest('tr');
    if (fila.hasClass('child')) fila = fila.prev();

    let data = tablaData.row(fila).data();
    idEditar = data.idDirector;

    // Llamamos a la función: 
    // data.IdUnidadEducativa para que la incluya en la consulta SQL
    // data.IdUnidadEducativa de nuevo para que el select se posicione en ella al terminar
    cargarUndidadesEd(data.idUnidadEducativa, data.idUnidadEducativa);

    $("#txtNombres").val(data.nombres);
    $("#txtApellidos").val(data.apellidos);
    $("#txtCorreo").val(data.correo);
    $("#txtCelular").val(data.celular);
    $("#txtNroci").val(data.nroCi);
    $("#cboEstado").val(data.estado ? 1 : 0).prop("disabled", false);

    $("#previewImg3").attr("src", data.photo || "/imagen/sinimagen.png");
    $("#fileInput3").val("");
    $('#file3Remove').addClass('d-none');

    $("#myModalLabel").text("Editar Registro");
    $("#mdUsuario").modal("show");
});

$("#btnNuevoUsuario").on("click", function () {
    idEditar = 0;

    // Limpia todos los inputs que tengan la clase 'model'
    $(".model").val("");
    cargarUndidadesEd(0, "");
    $("#cboEstado").val(1).prop("disabled", true);
    $('#file3Remove').addClass('d-none');

    $("#previewImg3").attr("src", "/imagen/sinimagen.png");
    $("#fileInput3").val("");

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

    let idSeleccionado = $("#cboUnidadEd").val();

    if (idSeleccionado === "") {
        MensajeToast("Campo incompleto", "Debe seleccionar la U.E.", "warning");
        habilitarBoton();
        return;
    }

    const modelo = {
        IdDirector: idEditar,
        IdUnidadEducativa: parseInt(idSeleccionado),
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

    $("#mdUsuario").find("div.modal-content").LoadingOverlay("show");

    let urlAccion = idEditar != 0 ? `/${controlador}/Editar` : `/${controlador}/Guardar`;
    let metodoApi = idEditar != 0 ? "PUT" : "POST";

    fetch(urlAccion, {
        method: metodoApi,
        body: formData
    })
        .then(response => response.ok ? response.json() : Promise.reject(response))
        .then(responseJson => {
            if (responseJson.estado) {
                MensajeToast("Mensaje", responseJson.mensaje, "success");
                $("#mdUsuario").modal("hide");
                tablaData.ajax.reload();
            } else {
                mostrarAlerta("Error!", responseJson.mensaje, "warning");
            }
        })
        .catch(() => {
            mostrarAlerta("Error!", "No se pudo conectar con el servidor.", "warning");
        })
        .finally(() => {
            $("#mdUsuario").find("div.modal-content").LoadingOverlay("hide");
            habilitarBoton();
        });

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