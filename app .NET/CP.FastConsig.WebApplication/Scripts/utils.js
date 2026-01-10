function calculamesfim(mesano, prazo) {
    var mes = parseInt(mesano.substr(0, 2));
    var ano = parseInt(mesano.substr(3, 4));

    var contador = 1;
    while (contador < prazo) {
        mes = mes + 1;
        if (mes > 12) {
            ano = ano + 1;
            mes = 1;
        }
        contador = contador + 1;
    }

    if (mes.toString().length < 2)
        mes = '0' + mes.toString();

    return mes.toString() + '/' + ano.toString();
}