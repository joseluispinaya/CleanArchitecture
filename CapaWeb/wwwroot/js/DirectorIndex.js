
let tablaData;
let idEditar = 0;
const controlador = "Directores";

$(function () {
    obtenerDirector();
    obtenerDirectorConvertido();
    //obtenerListaDirectores();

});

function obtenerDirector() {
    fetch(`/${controlador}/ObtenerDirector?idDirector=1`, {
        method: "PUT"
    })
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            // Muestra el objeto JSON completo devuelto por tu controlador
            console.log("Director obtenido:", responseJson);

            // Si quieres ver específicamente la lista de datos:
            // console.log("Lista de Tipos:", responseJson.data);
        })
        .catch((error) => {
            console.error("Error en la petición fetch:", error);
        });
}

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

function obtenerListaDirectores() {
    fetch(`/${controlador}/ListaDirectoresNormal`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    })
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            // Muestra el objeto JSON completo devuelto por tu controlador
            console.log("Lista Normal:", responseJson);

            // Si quieres ver específicamente la lista de datos:
            // console.log("Lista de Tipos:", responseJson.data);
        })
        .catch((error) => {
            console.error("Error en la petición fetch:", error);
        });
}

function obtenerListaDirectoresConvert() {
    fetch(`/${controlador}/ListaDirectoresConvert`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    })
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            // Muestra el objeto JSON completo devuelto por tu controlador
            console.log("Lista Convertida:", responseJson);

            // Si quieres ver específicamente la lista de datos:
            // console.log("Lista de Tipos:", responseJson.data);
        })
        .catch((error) => {
            console.error("Error en la petición fetch:", error);
        });
}

// fin