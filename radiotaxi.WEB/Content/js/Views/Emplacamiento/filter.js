// ===============================
// Helpers
// ===============================

// dd/MM/yyyy -> Date (local)
function parseDDMMYYYY(s) {
    if (!s) return null;
    const parts = s.split('/');
    if (parts.length !== 3) return null;

    const dd = parseInt(parts[0], 10);
    const mm = parseInt(parts[1], 10);
    const yyyy = parseInt(parts[2], 10);

    if (!dd || !mm || !yyyy) return null;
    return new Date(yyyy, mm - 1, dd);
}

// Regex exact match
function exactMatch(val) {
    const escaped = (val ?? '').toString().replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    return `^${escaped}$`;
}

// "Económico" -> int (soporta prefijos, comas, etc.)
function toIntEco(value) {
    const s = (value ?? '').toString().trim();
    if (!s) return null;
    const digits = s.replace(/[^\d]/g, '');
    if (!digits) return null;
    return parseInt(digits, 10);
}

// Normaliza texto (por si viene con espacios raros)
function normText(v) {
    return (v ?? '').toString().trim();
}

// ===============================
// Modelo dependiente de Marca
// ===============================
function resetModeloSelect() {
    const sel = document.querySelector('#fModelo');
    if (!sel) return;
    sel.innerHTML = '<option value="">Todos</option>';
    sel.value = '';
    sel.disabled = true;
}

function fillModelosByMarcaDT(dt, marca) {
    const sel = document.querySelector('#fModelo');
    if (!sel) return;

    sel.innerHTML = '<option value="">Todos</option>';

    if (!marca) {
        sel.disabled = true;
        return;
    }

    const modelos = new Set();

    // Usamos todas las filas (sin el search global), para que el combo no cambie raro.
    // Si quieres que respete filtros aplicados, cambia search:'none' por filter:'applied'
    dt.rows({ search: 'none' }).every(function () {
        const row = this.data(); // array columnas
        const rowMarca = normText(row[1]);
        const rowModelo = normText(row[2]);

        if (rowMarca === marca && rowModelo) modelos.add(rowModelo);
    });

    [...modelos]
        .sort((a, b) => a.localeCompare(b, 'es'))
        .forEach(m => {
            const opt = document.createElement('option');
            opt.value = m;
            opt.textContent = m;
            sel.appendChild(opt);
        });

    sel.disabled = false;
}

// ===============================
// Custom filters (DataTable.ext.search)
// ===============================

// 1) Date range filter (col 5)
DataTable.ext.search.push(function (settings, data) {
    if (settings.nTable && settings.nTable.id !== 'general') return true;

    const fechaTxt = data[5]; // dd/MM/yyyy
    const rowDate = parseDDMMYYYY(fechaTxt);
    if (!rowDate) return true;

    const desdeVal = document.querySelector('#fDesde')?.value; // yyyy-mm-dd
    const hastaVal = document.querySelector('#fHasta')?.value;

    const desde = desdeVal ? new Date(desdeVal + 'T00:00:00') : null;
    const hasta = hastaVal ? new Date(hastaVal + 'T23:59:59') : null;

    if (desde && rowDate < desde) return false;
    if (hasta && rowDate > hasta) return false;

    return true;
});

// 2) Tipo Económico filter (col 0)
DataTable.ext.search.push(function (settings, data) {
    if (settings.nTable && settings.nTable.id !== 'general') return true;

    const tipo = document.querySelector('#fTipoEco')?.value || '';
    if (!tipo) return true; // Todos

    const eco = toIntEco(data[0]);
    if (eco === null) return false;

    if (tipo === 'rut') return eco >= 1 && eco <= 10000;
    if (tipo === 'tte') return eco >= 10001 && eco <= 15000;
    if (tipo === 'esp') return eco >= 80001 && eco <= 80300;

    return true;
});

// ===============================
// Totales arriba y abajo
// ===============================
function updateSummaries(api) {
    const top = document.querySelector('#dtSummaryTop');
    const bottom = document.querySelector('#dtSummaryBottom');

    if (!top && !bottom) return;

    const totalAll = api.rows().count();
    const totalFiltered = api.rows({ filter: 'applied' }).count();

    const statusCounts = {};
    api.column(6, { filter: 'applied' }).data().each(v => {
        const s = normText(v) || 'Sin status';
        statusCounts[s] = (statusCounts[s] || 0) + 1;
    });

    const chips = Object.entries(statusCounts)
        .sort((a, b) => b[1] - a[1])
        .map(([s, n]) => `<span class="badge bg-label-primary me-2 mb-1">${s}: ${n}</span>`)
        .join(' ');

    const html = `
    <div class="card border-0 shadow-none mb-0">
      <div class="card-body p-3">
        <div class="d-flex flex-wrap gap-2 align-items-center justify-content-between">
          <div>
            <div class="fw-semibold">Resumen</div>
            <div class="text-body small">
              Total: <b>${totalAll}</b> · Mostrando (filtrado): <b>${totalFiltered}</b>
            </div>
          </div>
          <div class="d-flex flex-wrap justify-content-end">
            ${chips || '<span class="text-body small">Sin resultados con los filtros actuales.</span>'}
          </div>
        </div>
      </div>
    </div>
  `;

    if (top) top.innerHTML = html;
    if (bottom) bottom.innerHTML = html;
}

// ===============================
// DataTable init
// ===============================
const table = new DataTable('#general', {
    responsive: true,
    autoWidth: false,

    paging: true,
    searching: true,
    ordering: true,
    info: true,

    order: [[5, 'desc']], // fecha desc

    pageLength: 25,
    lengthMenu: [10, 25, 50, 100],

    // Badges en Status (col 6)
    columnDefs: [{
        targets: 6,
        render: (data, type) => {
            const s = normText(data);
            if (type !== 'display') return s;

            const map = {
                'EMPLACADO': 'bg-success',
                'Pendiente': 'bg-warning',
                'BAJA': 'bg-danger',
                'HISTORICO': 'bg-info'
            };
            const cls = map[s] ?? 'bg-secondary';
            return `<span class="badge ${cls}">${s}</span>`;
        }
    }],

    layout: {
        topStart: { buttons: ['excelHtml5', 'pdfHtml5'] },
        topEnd: 'search',
        bottomStart: 'info',
        bottomEnd: 'paging'
    },

    language: {
        search: "Buscar:",
        lengthMenu: "Mostrar _MENU_ registros",
        info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
        infoEmpty: "Mostrando 0 a 0 de 0 registros",
        infoFiltered: "(filtrado de _MAX_ registros totales)",
        zeroRecords: "No se encontraron registros",
        emptyTable: "No hay datos disponibles en la tabla",
        processing: "Procesando...",
        loadingRecords: "Cargando...",
        paginate: { first: "Primero", last: "Último", next: "Siguiente", previous: "Anterior" },
        buttons: {
            pageLength: { _: "Mostrar %d filas", '-1': "Mostrar todos" },
            excel: "Excel",
            pdf: "PDF"
        }
    },

    initComplete: function () {
        const api = this.api();

        // columnas: Marca=1, Modelo=2, Año=4, Status=6
        const colMarca = api.column(1);
        const colAno = api.column(4);
        const colStatus = api.column(6);

        function fillSelect(selectId, columnApi, sortNumeric = false, sortDesc = false) {
            const sel = document.querySelector(selectId);
            if (!sel) return;

            const values = new Set();
            columnApi.data().each(v => {
                const txt = normText(v);
                if (txt) values.add(txt);
            });

            let arr = [...values];

            if (sortNumeric) {
                arr.sort((a, b) => (parseInt(a, 10) || 0) - (parseInt(b, 10) || 0));
                if (sortDesc) arr.reverse();
            } else {
                arr.sort((a, b) => a.localeCompare(b, 'es'));
                if (sortDesc) arr.reverse();
            }

            arr.forEach(v => {
                const opt = document.createElement('option');
                opt.value = v;
                opt.textContent = v;
                sel.appendChild(opt);
            });
        }

        fillSelect('#fMarca', colMarca);
        fillSelect('#fAno', colAno, true, true); // años desc
        fillSelect('#fStatus', colStatus);

        // Modelo inicia vacío / deshabilitado hasta elegir marca
        resetModeloSelect();

        updateSummaries(api);
    },

    drawCallback: function () {
        updateSummaries(this.api());
    }
});

// ===============================
// Events: filtros por columna
// ===============================

// Marca -> filtra Marca + recarga Modelo dependiente
document.querySelector('#fMarca')?.addEventListener('change', (e) => {
    const marca = normText(e.target.value);

    // 1) Filtra Marca
    table.column(1).search(marca ? exactMatch(marca) : '', true, false);

    // 2) Limpia Modelo (filtro y select)
    table.column(2).search('', true, false);
    resetModeloSelect();

    // 3) Rellena modelos según marca
    fillModelosByMarcaDT(table, marca);

    table.draw();
});

// Modelo -> filtra Modelo
document.querySelector('#fModelo')?.addEventListener('change', (e) => {
    const modelo = normText(e.target.value);
    table.column(2).search(modelo ? exactMatch(modelo) : '', true, false).draw();
});

document.querySelector('#fAno')?.addEventListener('change', (e) => {
    const v = normText(e.target.value);
    table.column(4).search(v ? exactMatch(v) : '', true, false).draw();
});

document.querySelector('#fStatus')?.addEventListener('change', (e) => {
    const v = normText(e.target.value);
    table.column(6).search(v ? exactMatch(v) : '', true, false).draw();
});

// Tipo Económico (rut/tte/esp)
document.querySelector('#fTipoEco')?.addEventListener('change', () => {
    table.draw(); // ext.search
});

// Rango de fechas
['#fDesde', '#fHasta'].forEach(id => {
    document.querySelector(id)?.addEventListener('change', () => table.draw());
});

// Reset
document.querySelector('#fReset')?.addEventListener('click', () => {
    const ids = ['#fMarca', '#fModelo', '#fAno', '#fStatus', '#fDesde', '#fHasta', '#fTipoEco'];
    ids.forEach(id => {
        const el = document.querySelector(id);
        if (el) el.value = '';
    });

    resetModeloSelect();

    table.column(1).search('', true, false);
    table.column(2).search('', true, false);
    table.column(4).search('', true, false);
    table.column(6).search('', true, false);
    table.search('');
    table.draw();
});
