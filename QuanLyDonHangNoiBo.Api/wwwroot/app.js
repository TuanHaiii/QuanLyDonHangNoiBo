const i18n = {
  vi: {
    'nav.dashboard': 'Dashboard',
    'nav.orders': 'Don hang',
    'nav.inventory': 'Ton kho',
    'nav.delivery': 'Giao hang',
    'nav.finance': 'COD & Tai chinh',
    'nav.catalog': 'Danh muc',
    'nav.ai': 'AI Assistant',
    'nav.settings': 'Cai dat',
    'common.refresh': 'Lam moi',
    'common.search': 'Tim kiem',
    'common.status': 'Trang thai',
    'common.warehouse': 'Kho',
    'common.all': 'Tat ca',
    'common.actions': 'Thao tac',
    'common.customer': 'Khach hang',
    'common.total': 'Tong tien',
    'common.cod': 'COD',
    'common.createdAt': 'Ngay tao',
    'common.save': 'Luu',
    'common.cancel': 'Huy',
    'common.detail': 'Chi tiet',
    'settings.language': 'Ngon ngu',
    'view.dashboard.title': 'Dashboard van hanh',
    'view.dashboard.subtitle': 'KPI, canh bao, pipeline don hang va AI insight theo tenant.',
    'view.orders.title': 'Quan ly don hang',
    'view.orders.subtitle': 'Tao don, xac nhan, pick/pack, xuat kho va cap nhat trang thai.',
    'view.inventory.title': 'Ton kho va SKU',
    'view.inventory.subtitle': 'Theo doi on-hand, reserved, available va canh bao sap het hang.',
    'view.delivery.title': 'Dieu phoi giao hang',
    'view.delivery.subtitle': 'Gan tai xe, theo doi shipment va cap nhat giao thanh cong/that bai.',
    'view.finance.title': 'COD & doi soat',
    'view.finance.subtitle': 'Theo doi tien thu ho, chenhlech va trang thai reconcile.',
    'view.catalog.title': 'Khach hang va san pham',
    'view.catalog.subtitle': 'Du lieu danh muc dung cho tao don va van hanh kho.',
    'view.ai.title': 'AI Assistant',
    'view.ai.subtitle': 'Hoi nhanh ve don hang, COD, ton kho va rui ro giao hang.',
    'view.settings.title': 'Cau hinh he thong',
    'view.settings.subtitle': 'Tenant, user, thong bao va tuy chon hien thi.',
    'status.Draft': 'Nhap',
    'status.Confirmed': 'Da xac nhan',
    'status.WaitingPick': 'Cho pick',
    'status.Picking': 'Dang pick',
    'status.Packed': 'Da dong goi',
    'status.ReadyToShip': 'San sang giao',
    'status.InTransit': 'Dang giao',
    'status.Delivered': 'Da giao',
    'status.Failed': 'Giao that bai',
    'status.Cancelled': 'Da huy',
    'status.Returned': 'Hoan hang',
    'shipment.Pending': 'Cho gan',
    'shipment.Assigned': 'Da gan',
    'shipment.Accepted': 'Tai xe nhan',
    'shipment.PickingUp': 'Dang lay hang',
    'shipment.InTransit': 'Dang giao',
    'shipment.Delivered': 'Da giao',
    'shipment.Failed': 'That bai',
    'shipment.Returned': 'Hoan hang',
    'cod.Pending': 'Cho thu',
    'cod.Collected': 'Da thu',
    'cod.Reconciled': 'Da doi soat',
    'cod.Mismatch': 'Lech tien'
  },
  en: {
    'nav.dashboard': 'Dashboard',
    'nav.orders': 'Orders',
    'nav.inventory': 'Inventory',
    'nav.delivery': 'Delivery',
    'nav.finance': 'COD & Finance',
    'nav.catalog': 'Catalog',
    'nav.ai': 'AI Assistant',
    'nav.settings': 'Settings',
    'common.refresh': 'Refresh',
    'common.search': 'Search',
    'common.status': 'Status',
    'common.warehouse': 'Warehouse',
    'common.all': 'All',
    'common.actions': 'Actions',
    'common.customer': 'Customer',
    'common.total': 'Total',
    'common.cod': 'COD',
    'common.createdAt': 'Created',
    'common.save': 'Save',
    'common.cancel': 'Cancel',
    'common.detail': 'Detail',
    'settings.language': 'Language',
    'view.dashboard.title': 'Operations dashboard',
    'view.dashboard.subtitle': 'Tenant KPIs, alerts, order pipeline, and AI insights.',
    'view.orders.title': 'Order management',
    'view.orders.subtitle': 'Create, confirm, pick, pack, ship, and update order statuses.',
    'view.inventory.title': 'Inventory and SKU',
    'view.inventory.subtitle': 'Track on-hand, reserved, available stock, and low-stock alerts.',
    'view.delivery.title': 'Delivery dispatch',
    'view.delivery.subtitle': 'Assign drivers, track shipments, and update delivery outcomes.',
    'view.finance.title': 'COD reconciliation',
    'view.finance.subtitle': 'Track cash-on-delivery collections, mismatches, and reconcile status.',
    'view.catalog.title': 'Customers and products',
    'view.catalog.subtitle': 'Master data used by order creation and warehouse operations.',
    'view.ai.title': 'AI Assistant',
    'view.ai.subtitle': 'Ask about orders, COD, inventory, and delivery risks.',
    'view.settings.title': 'System settings',
    'view.settings.subtitle': 'Tenant, users, notifications, and display options.',
    'status.Draft': 'Draft',
    'status.Confirmed': 'Confirmed',
    'status.WaitingPick': 'Waiting pick',
    'status.Picking': 'Picking',
    'status.Packed': 'Packed',
    'status.ReadyToShip': 'Ready to ship',
    'status.InTransit': 'In transit',
    'status.Delivered': 'Delivered',
    'status.Failed': 'Failed',
    'status.Cancelled': 'Cancelled',
    'status.Returned': 'Returned',
    'shipment.Pending': 'Pending',
    'shipment.Assigned': 'Assigned',
    'shipment.Accepted': 'Accepted',
    'shipment.PickingUp': 'Picking up',
    'shipment.InTransit': 'In transit',
    'shipment.Delivered': 'Delivered',
    'shipment.Failed': 'Failed',
    'shipment.Returned': 'Returned',
    'cod.Pending': 'Pending',
    'cod.Collected': 'Collected',
    'cod.Reconciled': 'Reconciled',
    'cod.Mismatch': 'Mismatch'
  }
};

const state = {
  locale: localStorage.getItem('oms.locale') || 'vi',
  view: location.hash.replace('#', '') || 'dashboard',
  tenant: null,
  user: null,
  metadata: null,
  selectedOrderId: null,
  orderFilters: { search: '', status: '', warehouseId: '' },
  inventoryFilters: { search: '', warehouseId: '' },
  chat: []
};

const app = document.getElementById('app');

document.addEventListener('DOMContentLoaded', init);

async function init() {
  bindChrome();
  applyI18n();
  await loginDemo();
  await loadMetadata();
  await renderView(state.view);
}

function bindChrome() {
  document.querySelectorAll('.nav-item').forEach((button) => {
    button.addEventListener('click', () => {
      state.view = button.dataset.view;
      location.hash = state.view;
      renderView(state.view);
    });
  });

  window.addEventListener('hashchange', () => {
    const nextView = location.hash.replace('#', '') || 'dashboard';
    if (nextView !== state.view) {
      state.view = nextView;
      renderView(nextView);
    }
  });

  document.getElementById('refreshButton').addEventListener('click', () => renderView(state.view));
  document.getElementById('localeSelect').value = state.locale;
  document.getElementById('localeSelect').addEventListener('change', (event) => {
    state.locale = event.target.value;
    localStorage.setItem('oms.locale', state.locale);
    applyI18n();
    renderView(state.view);
  });
}

function applyI18n() {
  document.documentElement.lang = state.locale;
  document.querySelectorAll('[data-i18n]').forEach((node) => {
    node.textContent = t(node.dataset.i18n);
  });
  document.getElementById('viewTitle').textContent = t(`view.${state.view}.title`);
  document.getElementById('viewSubtitle').textContent = t(`view.${state.view}.subtitle`);
}

function t(key) {
  return i18n[state.locale]?.[key] || i18n.vi[key] || key;
}

async function loginDemo() {
  const response = await api('/api/auth/login', {
    method: 'POST',
    body: {
      tenantCode: 'demo',
      email: 'admin@demo.vn',
      password: 'demo',
      locale: state.locale
    }
  });
  state.tenant = response.tenant;
  state.user = response.user;
  document.getElementById('tenantName').textContent = state.tenant.name;
  document.getElementById('userName').textContent = state.user.fullName;
  document.getElementById('userRole').textContent = state.user.role;
}

async function loadMetadata() {
  state.metadata = await api('/api/metadata');
}

async function renderView(view) {
  state.view = view || state.view;
  applyI18n();
  document.querySelectorAll('.nav-item').forEach((button) => {
    button.classList.toggle('is-active', button.dataset.view === state.view);
  });
  app.innerHTML = '<div class="loader"></div>';

  try {
    if (state.view === 'dashboard') await renderDashboard();
    if (state.view === 'orders') await renderOrders();
    if (state.view === 'inventory') await renderInventory();
    if (state.view === 'delivery') await renderDelivery();
    if (state.view === 'finance') await renderFinance();
    if (state.view === 'catalog') await renderCatalog();
    if (state.view === 'ai') await renderAi();
    if (state.view === 'settings') await renderSettings();
  } catch (error) {
    app.innerHTML = `<div class="empty">${escapeHtml(error.message)}</div>`;
    showToast(error.message);
  }
}

async function renderDashboard() {
  const dashboard = await api('/api/dashboard');
  const maxStatus = Math.max(...dashboard.statusBreakdown.map((item) => item.count), 1);

  app.innerHTML = `
    <div class="grid grid-4">
      ${dashboard.kpis.map((kpi) => `
        <article class="kpi-card">
          <span>${escapeHtml(kpi.label)}</span>
          <strong>${escapeHtml(kpi.value)}</strong>
          <em class="kpi-delta">${kpi.delta >= 0 ? '+' : ''}${kpi.delta}%</em>
        </article>
      `).join('')}
    </div>

    <div class="grid grid-2" style="margin-top:16px">
      <section class="panel">
        <header class="panel-header"><h2>Pipeline</h2></header>
        <div class="panel-body pipeline">
          ${dashboard.pipeline.map((column) => `
            <div class="pipeline-column">
              <header><span>${statusText(column.status)}</span><b>${column.count}</b></header>
              ${column.orders.length ? column.orders.map(renderMiniOrder).join('') : '<div class="empty">0</div>'}
            </div>
          `).join('')}
        </div>
      </section>

      <section class="panel">
        <header class="panel-header"><h2>${t('common.status')}</h2></header>
        <div class="panel-body">
          ${dashboard.statusBreakdown.map((item) => `
            <div class="chart-row">
              <span>${statusText(item.status)}</span>
              <div class="bar"><span style="width:${Math.max(4, (item.count / maxStatus) * 100)}%"></span></div>
              <b>${item.count}</b>
            </div>
          `).join('')}
        </div>
      </section>
    </div>

    <div class="grid grid-3" style="margin-top:16px">
      <section class="panel">
        <header class="panel-header"><h2>Alerts</h2></header>
        <div class="panel-body stack">
          ${dashboard.alerts.length ? dashboard.alerts.map((item) => `
            <article class="alert-item ${item.severity === 'Critical' ? 'critical' : ''}">
              <h3>${escapeHtml(item.title)}</h3>
              <p class="muted">${escapeHtml(item.message)}</p>
            </article>
          `).join('') : '<div class="empty">OK</div>'}
        </div>
      </section>

      <section class="panel">
        <header class="panel-header"><h2>AI Insight</h2></header>
        <div class="panel-body stack">
          ${dashboard.aiInsights.map((item) => `
            <article class="insight-item">
              <span class="badge ${severityClass(item.severity)}">${escapeHtml(item.severity)}</span>
              <h3 style="margin-top:8px">${escapeHtml(item.title)}</h3>
              <p class="muted">${escapeHtml(item.summary)}</p>
            </article>
          `).join('')}
        </div>
      </section>

      <section class="panel">
        <header class="panel-header"><h2>${t('nav.orders')}</h2></header>
        <div class="panel-body stack">
          ${dashboard.recentOrders.slice(0, 5).map(renderMiniOrder).join('')}
        </div>
      </section>
    </div>
  `;
}

async function renderOrders() {
  const params = new URLSearchParams({ pageSize: '50' });
  if (state.orderFilters.search) params.set('search', state.orderFilters.search);
  if (state.orderFilters.status) params.set('status', state.orderFilters.status);
  if (state.orderFilters.warehouseId) params.set('warehouseId', state.orderFilters.warehouseId);
  const result = await api(`/api/orders?${params}`);
  const orders = result.items;
  if (!state.selectedOrderId && orders.length) state.selectedOrderId = orders[0].id;

  app.innerHTML = `
    <div class="split">
      <section class="panel">
        <header class="panel-header">
          <h2>${t('nav.orders')}</h2>
          <button id="createOrderButton" class="button button-primary">Tao don</button>
        </header>
        <div class="panel-body">
          <div class="toolbar">
            <input id="orderSearch" class="input" style="max-width:260px" value="${escapeAttr(state.orderFilters.search)}" placeholder="${t('common.search')}">
            <select id="orderStatus" class="select" style="max-width:190px">
              <option value="">${t('common.all')}</option>
              ${state.metadata.orderStatuses.map((status) => `<option value="${status}" ${state.orderFilters.status === status ? 'selected' : ''}>${statusText(status)}</option>`).join('')}
            </select>
            <select id="orderWarehouse" class="select" style="max-width:190px">
              <option value="">${t('common.warehouse')}</option>
              ${state.metadata.warehouses.map((item) => `<option value="${item.id}" ${state.orderFilters.warehouseId === item.id ? 'selected' : ''}>${escapeHtml(item.code)} - ${escapeHtml(item.name)}</option>`).join('')}
            </select>
            <button id="applyOrderFilter" class="button button-secondary">${t('common.search')}</button>
          </div>
          <div class="table-wrap">
            <table class="table">
              <thead>
                <tr>
                  <th>Ma don</th>
                  <th>${t('common.customer')}</th>
                  <th>${t('common.status')}</th>
                  <th>${t('common.total')}</th>
                  <th>${t('common.cod')}</th>
                  <th>${t('common.actions')}</th>
                </tr>
              </thead>
              <tbody>
                ${orders.map((order) => `
                  <tr>
                    <td><button class="button button-secondary button-small" data-detail="${order.id}">${escapeHtml(order.orderCode)}</button></td>
                    <td>${escapeHtml(order.customerName)}<br><span class="muted">${escapeHtml(order.customerPhone)}</span></td>
                    <td>${statusBadge(order.status)}</td>
                    <td class="money">${formatMoney(order.total)}</td>
                    <td class="money">${formatMoney(order.codAmount)}</td>
                    <td><div class="actions">${renderOrderActions(order)}</div></td>
                  </tr>
                `).join('')}
              </tbody>
            </table>
          </div>
        </div>
      </section>

      <aside class="stack">
        <section id="createOrderPanel" class="panel" style="display:none">
          <header class="panel-header"><h2>Tao don hang</h2></header>
          <div class="panel-body">${renderCreateOrderForm()}</div>
        </section>
        <section id="orderDetailPanel" class="panel detail-card">
          <div class="panel-body"><div class="loader"></div></div>
        </section>
      </aside>
    </div>
  `;

  bindOrderEvents();
  if (state.selectedOrderId) await loadOrderDetail(state.selectedOrderId);
}

function bindOrderEvents() {
  document.getElementById('applyOrderFilter').addEventListener('click', () => {
    state.orderFilters.search = document.getElementById('orderSearch').value.trim();
    state.orderFilters.status = document.getElementById('orderStatus').value;
    state.orderFilters.warehouseId = document.getElementById('orderWarehouse').value;
    renderOrders();
  });

  document.getElementById('createOrderButton').addEventListener('click', () => {
    const panel = document.getElementById('createOrderPanel');
    panel.style.display = panel.style.display === 'none' ? 'block' : 'none';
  });

  document.querySelectorAll('[data-detail]').forEach((button) => {
    button.addEventListener('click', () => {
      state.selectedOrderId = button.dataset.detail;
      loadOrderDetail(state.selectedOrderId);
    });
  });

  document.querySelectorAll('[data-order-action]').forEach((button) => {
    button.addEventListener('click', () => changeOrderStatus(button.dataset.orderId, button.dataset.orderAction));
  });

  document.getElementById('createOrderForm').addEventListener('submit', createOrder);
}

async function loadOrderDetail(orderId) {
  const panel = document.getElementById('orderDetailPanel');
  panel.innerHTML = '<div class="panel-body"><div class="loader"></div></div>';
  const order = await api(`/api/orders/${orderId}`);
  panel.innerHTML = `
    <header class="panel-header">
      <div>
        <h2>${escapeHtml(order.orderCode)}</h2>
        <span class="muted">${formatDate(order.createdAt)}</span>
      </div>
      ${statusBadge(order.status)}
    </header>
    <div class="panel-body stack">
      <div>
        <h3>${escapeHtml(order.customer.name)}</h3>
        <p class="muted">${escapeHtml(order.deliveryAddress)}</p>
      </div>
      <div class="table-wrap">
        <table class="table" style="min-width:420px">
          <tbody>
            ${order.items.map((item) => `
              <tr>
                <td>${escapeHtml(item.skuCode)}<br><span class="muted">${escapeHtml(item.productName)}</span></td>
                <td>${item.quantity}</td>
                <td class="money">${formatMoney(item.lineTotal)}</td>
              </tr>
            `).join('')}
          </tbody>
        </table>
      </div>
      <div class="grid grid-2">
        <div><span class="muted">Subtotal</span><br><b>${formatMoney(order.subtotal)}</b></div>
        <div><span class="muted">COD</span><br><b>${formatMoney(order.codAmount)}</b></div>
      </div>
      <div class="stack">
        ${order.history.map((item) => `<div class="muted">${formatDate(item.changedAt)} - ${statusText(item.oldStatus)} -> ${statusText(item.newStatus)}</div>`).join('')}
      </div>
    </div>
  `;
}

function renderCreateOrderForm() {
  return `
    <form id="createOrderForm" class="stack">
      <div class="field">
        <label class="field-label">Khach hang</label>
        <select id="newCustomer" class="select" required>
          ${state.metadata.customers.map((item) => `<option value="${item.id}">${escapeHtml(item.code)} - ${escapeHtml(item.name)}</option>`).join('')}
        </select>
      </div>
      <div class="field">
        <label class="field-label">Kho xuat</label>
        <select id="newWarehouse" class="select" required>
          ${state.metadata.warehouses.map((item) => `<option value="${item.id}">${escapeHtml(item.code)} - ${escapeHtml(item.name)}</option>`).join('')}
        </select>
      </div>
      <div class="form-grid">
        <div class="field">
          <label class="field-label">SKU</label>
          <select id="newSku" class="select" required>
            ${state.metadata.skus.map((item) => `<option value="${item.id}">${escapeHtml(item.code)} - ${escapeHtml(item.name)}</option>`).join('')}
          </select>
        </div>
        <div class="field">
          <label class="field-label">So luong</label>
          <input id="newQty" class="input" type="number" min="1" value="1" required>
        </div>
      </div>
      <div class="form-grid">
        <div class="field">
          <label class="field-label">Phi ship</label>
          <input id="newShipping" class="input" type="number" min="0" value="25000">
        </div>
        <div class="field">
          <label class="field-label">Giam gia</label>
          <input id="newDiscount" class="input" type="number" min="0" value="0">
        </div>
      </div>
      <div class="field">
        <label class="field-label">Ghi chu noi bo</label>
        <textarea id="newNote" class="textarea"></textarea>
      </div>
      <button class="button button-primary" type="submit">${t('common.save')}</button>
    </form>
  `;
}

async function createOrder(event) {
  event.preventDefault();
  const body = {
    customerId: document.getElementById('newCustomer').value,
    warehouseId: document.getElementById('newWarehouse').value,
    paymentMethod: 'Cod',
    shippingFee: Number(document.getElementById('newShipping').value || 0),
    discount: Number(document.getElementById('newDiscount').value || 0),
    internalNote: document.getElementById('newNote').value,
    items: [{
      skuId: document.getElementById('newSku').value,
      quantity: Number(document.getElementById('newQty').value || 1)
    }]
  };
  const order = await api('/api/orders', { method: 'POST', body });
  state.selectedOrderId = order.id;
  showToast(`Created ${order.orderCode}`);
  await renderOrders();
}

async function changeOrderStatus(orderId, action) {
  if (action === 'confirm') {
    await api(`/api/orders/${orderId}/confirm`, { method: 'POST', body: { userId: state.user.id } });
  } else if (action === 'cancel') {
    await api(`/api/orders/${orderId}/cancel`, { method: 'POST', body: { userId: state.user.id, note: 'Cancelled from web UI' } });
  } else {
    await api(`/api/orders/${orderId}/status`, { method: 'POST', body: { status: action, note: 'Updated from web UI', userId: state.user.id } });
  }
  showToast('Updated');
  await renderOrders();
}

async function renderInventory() {
  const params = new URLSearchParams();
  if (state.inventoryFilters.search) params.set('search', state.inventoryFilters.search);
  if (state.inventoryFilters.warehouseId) params.set('warehouseId', state.inventoryFilters.warehouseId);
  const stocks = await api(`/api/inventory/stocks?${params}`);

  app.innerHTML = `
    <section class="panel">
      <header class="panel-header"><h2>${t('nav.inventory')}</h2></header>
      <div class="panel-body">
        <div class="toolbar">
          <input id="inventorySearch" class="input" style="max-width:260px" value="${escapeAttr(state.inventoryFilters.search)}" placeholder="${t('common.search')}">
          <select id="inventoryWarehouse" class="select" style="max-width:220px">
            <option value="">${t('common.all')}</option>
            ${state.metadata.warehouses.map((item) => `<option value="${item.id}" ${state.inventoryFilters.warehouseId === item.id ? 'selected' : ''}>${escapeHtml(item.code)} - ${escapeHtml(item.name)}</option>`).join('')}
          </select>
          <button id="applyInventoryFilter" class="button button-secondary">${t('common.search')}</button>
        </div>
        <div class="table-wrap">
          <table class="table">
            <thead>
              <tr>
                <th>SKU</th>
                <th>${t('common.warehouse')}</th>
                <th>On hand</th>
                <th>Reserved</th>
                <th>Available</th>
                <th>Threshold</th>
                <th>${t('common.status')}</th>
              </tr>
            </thead>
            <tbody>
              ${stocks.map((stock) => `
                <tr>
                  <td>${escapeHtml(stock.skuCode)}<br><span class="muted">${escapeHtml(stock.productName)}</span></td>
                  <td>${escapeHtml(stock.warehouseCode)}<br><span class="muted">${escapeHtml(stock.warehouseName)}</span></td>
                  <td>${stock.onHand}</td>
                  <td>${stock.reserved}</td>
                  <td><b>${stock.available}</b></td>
                  <td>${stock.lowStockThreshold}</td>
                  <td>${stock.isLowStock ? '<span class="badge danger">Low stock</span>' : '<span class="badge success">OK</span>'}</td>
                </tr>
              `).join('')}
            </tbody>
          </table>
        </div>
      </div>
    </section>
  `;
  document.getElementById('applyInventoryFilter').addEventListener('click', () => {
    state.inventoryFilters.search = document.getElementById('inventorySearch').value.trim();
    state.inventoryFilters.warehouseId = document.getElementById('inventoryWarehouse').value;
    renderInventory();
  });
}

async function renderDelivery() {
  const shipments = await api('/api/delivery/shipments');
  app.innerHTML = `
    <div class="grid grid-3">
      ${shipments.map((shipment) => `
        <article class="shipment-card">
          <div>
            <h2>${escapeHtml(shipment.shipmentCode)}</h2>
            <p class="muted">${escapeHtml(shipment.orderCode)} - ${escapeHtml(shipment.customerName)}</p>
          </div>
          <div>${shipmentBadge(shipment.status)}</div>
          <div class="muted">${escapeHtml(shipment.deliveryAddress)}</div>
          <div class="money">COD ${formatMoney(shipment.codAmount)}</div>
          <div class="field">
            <label class="field-label">Tai xe</label>
            <select class="select" data-driver-select="${shipment.id}">
              ${state.metadata.drivers.map((driver) => `<option value="${driver.id}" ${shipment.driverId === driver.id ? 'selected' : ''}>${escapeHtml(driver.name)}</option>`).join('')}
            </select>
          </div>
          <div class="actions">
            <button class="button button-secondary button-small" data-assign-shipment="${shipment.id}">Assign</button>
            <button class="button button-secondary button-small" data-shipment-status="InTransit" data-shipment-id="${shipment.id}">${t('shipment.InTransit')}</button>
            <button class="button button-secondary button-small" data-shipment-status="Delivered" data-shipment-id="${shipment.id}">${t('shipment.Delivered')}</button>
            <button class="button button-danger button-small" data-shipment-status="Failed" data-shipment-id="${shipment.id}">${t('shipment.Failed')}</button>
          </div>
        </article>
      `).join('')}
    </div>
  `;

  document.querySelectorAll('[data-assign-shipment]').forEach((button) => {
    button.addEventListener('click', async () => {
      const id = button.dataset.assignShipment;
      const driverId = document.querySelector(`[data-driver-select="${id}"]`).value;
      await api(`/api/delivery/shipments/${id}/assign`, { method: 'POST', body: { driverId, routeName: 'Tuyen web dispatch' } });
      showToast('Assigned');
      await renderDelivery();
    });
  });

  document.querySelectorAll('[data-shipment-status]').forEach((button) => {
    button.addEventListener('click', async () => {
      await api(`/api/driver/shipments/${button.dataset.shipmentId}/status`, {
        method: 'POST',
        body: { status: button.dataset.shipmentStatus, note: 'Updated from dispatch board', collectedAmount: null }
      });
      showToast('Updated');
      await renderDelivery();
    });
  });
}

async function renderFinance() {
  const cods = await api('/api/cod/collections');
  app.innerHTML = `
    <section class="panel">
      <header class="panel-header"><h2>${t('nav.finance')}</h2></header>
      <div class="panel-body">
        <div class="table-wrap">
          <table class="table">
            <thead>
              <tr>
                <th>Order</th>
                <th>${t('common.customer')}</th>
                <th>Driver</th>
                <th>Expected</th>
                <th>Collected</th>
                <th>${t('common.status')}</th>
                <th>${t('common.actions')}</th>
              </tr>
            </thead>
            <tbody>
              ${cods.map((cod) => `
                <tr>
                  <td>${escapeHtml(cod.orderCode)}</td>
                  <td>${escapeHtml(cod.customerName)}</td>
                  <td>${escapeHtml(cod.driverName)}</td>
                  <td class="money">${formatMoney(cod.expectedAmount)}</td>
                  <td class="money">${formatMoney(cod.collectedAmount)}</td>
                  <td>${codBadge(cod.status)}</td>
                  <td>
                    ${cod.status !== 'Reconciled' ? `<button class="button button-secondary button-small" data-reconcile="${cod.id}">Reconcile</button>` : ''}
                  </td>
                </tr>
              `).join('')}
            </tbody>
          </table>
        </div>
      </div>
    </section>
  `;

  document.querySelectorAll('[data-reconcile]').forEach((button) => {
    button.addEventListener('click', async () => {
      await api('/api/cod/reconcile', { method: 'POST', body: { codCollectionIds: [button.dataset.reconcile], userId: state.user.id } });
      showToast('Reconciled');
      await renderFinance();
    });
  });
}

async function renderCatalog() {
  const [customers, products] = await Promise.all([api('/api/customers'), api('/api/products')]);
  app.innerHTML = `
    <div class="grid grid-2">
      <section class="panel">
        <header class="panel-header"><h2>Customers</h2></header>
        <div class="panel-body stack">
          ${customers.map((item) => `
            <article class="mini-order">
              <strong>${escapeHtml(item.name)}</strong>
              <span class="muted">${escapeHtml(item.code)} - ${escapeHtml(item.phone)}</span>
              <span>${escapeHtml(item.address)}</span>
            </article>
          `).join('')}
        </div>
      </section>
      <section class="panel">
        <header class="panel-header"><h2>Products & SKU</h2></header>
        <div class="panel-body stack">
          ${products.map((product) => `
            <article class="mini-order">
              <strong>${escapeHtml(product.name)}</strong>
              <span class="muted">${escapeHtml(product.code)} - ${escapeHtml(product.category)}</span>
              <div class="actions">
                ${product.skus.map((sku) => `<span class="badge info">${escapeHtml(sku.skuCode)} ${formatMoney(sku.price)}</span>`).join('')}
              </div>
            </article>
          `).join('')}
        </div>
      </section>
    </div>
  `;
}

async function renderAi() {
  app.innerHTML = `
    <div class="chat-layout">
      <section class="panel">
        <header class="panel-header"><h2>Chat</h2></header>
        <div class="panel-body stack">
          <div id="chatLog" class="chat-log">
            ${state.chat.length ? state.chat.map(renderChatMessage).join('') : '<div class="empty">AI Assistant</div>'}
          </div>
          <form id="aiForm" class="toolbar">
            <input id="aiQuestion" class="input" style="flex:1" placeholder="VD: COD nao chua doi soat?">
            <button class="button button-primary" type="submit">Ask</button>
          </form>
        </div>
      </section>
      <aside class="panel">
        <header class="panel-header"><h2>Suggested</h2></header>
        <div class="panel-body suggestions">
          ${['Hom nay co bao nhieu don giao tre?', 'Kho nao sap het hang?', 'COD nao chua doi soat?', 'Don nao co nguy co giao that bai?'].map((question) => `
            <button class="button button-secondary" data-ai-suggestion="${escapeAttr(question)}">${escapeHtml(question)}</button>
          `).join('')}
        </div>
      </aside>
    </div>
  `;

  document.getElementById('aiForm').addEventListener('submit', askAi);
  document.querySelectorAll('[data-ai-suggestion]').forEach((button) => {
    button.addEventListener('click', () => {
      document.getElementById('aiQuestion').value = button.dataset.aiSuggestion;
      document.getElementById('aiForm').requestSubmit();
    });
  });
}

async function askAi(event) {
  event.preventDefault();
  const input = document.getElementById('aiQuestion');
  const question = input.value.trim();
  if (!question) return;
  state.chat.push({ role: 'user', text: question });
  input.value = '';
  renderAi();
  const response = await api('/api/ai/chat', { method: 'POST', body: { tenantId: state.tenant.id, question, userId: state.user.id } });
  state.chat.push({ role: 'ai', text: response.answer, links: response.relatedLinks });
  renderAi();
}

async function renderSettings() {
  const [tenants, users, notifications] = await Promise.all([api('/api/tenants'), api('/api/users'), api('/api/notifications')]);
  app.innerHTML = `
    <div class="grid grid-3">
      <section class="panel">
        <header class="panel-header"><h2>Tenant</h2></header>
        <div class="panel-body stack">
          ${tenants.map((tenant) => `
            <article class="mini-order">
              <strong>${escapeHtml(tenant.name)}</strong>
              <span class="muted">${escapeHtml(tenant.code)} - ${escapeHtml(tenant.plan)}</span>
              <span>${escapeHtml(tenant.status)} - ${tenant.userLimit} users - ${tenant.orderLimit} orders</span>
            </article>
          `).join('')}
        </div>
      </section>
      <section class="panel">
        <header class="panel-header"><h2>Users</h2></header>
        <div class="panel-body stack">
          ${users.map((user) => `
            <article class="mini-order">
              <strong>${escapeHtml(user.fullName)}</strong>
              <span class="muted">${escapeHtml(user.email)}</span>
              <span class="badge info">${escapeHtml(user.role)}</span>
            </article>
          `).join('')}
        </div>
      </section>
      <section class="panel">
        <header class="panel-header"><h2>Notifications</h2></header>
        <div class="panel-body stack">
          ${notifications.map((item) => `
            <article class="alert-item ${item.severity === 'Critical' ? 'critical' : ''}">
              <h3>${escapeHtml(item.title)}</h3>
              <p class="muted">${escapeHtml(item.message)}</p>
            </article>
          `).join('')}
        </div>
      </section>
    </div>
  `;
}

function renderMiniOrder(order) {
  return `
    <article class="mini-order">
      <strong>${escapeHtml(order.orderCode)}</strong>
      <span>${escapeHtml(order.customerName)}</span>
      <span class="muted">${formatMoney(order.total)} - ${statusText(order.status)}</span>
    </article>
  `;
}

function renderOrderActions(order) {
  const actions = [];
  if (order.status === 'Draft') actions.push(['confirm', 'Confirm']);
  if (order.status === 'Confirmed') actions.push(['Picking', t('status.Picking')], ['cancel', t('common.cancel')]);
  if (order.status === 'Picking') actions.push(['Packed', t('status.Packed')]);
  if (order.status === 'Packed') actions.push(['ReadyToShip', t('status.ReadyToShip')]);
  if (order.status === 'ReadyToShip') actions.push(['InTransit', t('status.InTransit')]);
  if (order.status === 'InTransit') actions.push(['Delivered', t('status.Delivered')], ['Failed', t('status.Failed')]);

  return actions.map(([action, label]) => `
    <button class="button ${action === 'cancel' || action === 'Failed' ? 'button-danger' : 'button-secondary'} button-small" data-order-id="${order.id}" data-order-action="${action}">${escapeHtml(label)}</button>
  `).join('');
}

function renderChatMessage(message) {
  const links = message.links?.length
    ? `<div class="actions" style="margin-top:8px">${message.links.map((link) => `<button class="button button-secondary button-small" onclick="location.hash='${link.url.replace('/', '')}'">${escapeHtml(link.label)}</button>`).join('')}</div>`
    : '';
  return `<article class="chat-message ${message.role}">${escapeHtml(message.text)}${links}</article>`;
}

async function api(path, options = {}) {
  const config = {
    method: options.method || 'GET',
    headers: { 'Content-Type': 'application/json', ...(options.headers || {}) }
  };
  if (options.body !== undefined) {
    config.body = typeof options.body === 'string' ? options.body : JSON.stringify(options.body);
  }
  const response = await fetch(path, config);
  if (!response.ok) {
    const payload = await response.json().catch(() => null);
    throw new Error(payload?.message || response.statusText || 'Request failed');
  }
  return response.status === 204 ? null : response.json();
}

function statusText(status) {
  return t(`status.${status}`);
}

function shipmentText(status) {
  return t(`shipment.${status}`);
}

function codText(status) {
  return t(`cod.${status}`);
}

function statusBadge(status) {
  return `<span class="badge ${statusClass(status)}">${statusText(status)}</span>`;
}

function shipmentBadge(status) {
  return `<span class="badge ${statusClass(status)}">${shipmentText(status)}</span>`;
}

function codBadge(status) {
  return `<span class="badge ${statusClass(status)}">${codText(status)}</span>`;
}

function statusClass(status) {
  if (['Delivered', 'Reconciled', 'Collected'].includes(status)) return 'success';
  if (['Failed', 'Cancelled', 'Returned', 'Mismatch', 'Critical'].includes(status)) return 'danger';
  if (['InTransit', 'Confirmed', 'Assigned', 'Accepted'].includes(status)) return 'info';
  return 'warning';
}

function severityClass(severity) {
  if (severity === 'Critical') return 'danger';
  if (severity === 'Warning') return 'warning';
  if (severity === 'Success') return 'success';
  return 'info';
}

function formatMoney(value) {
  return new Intl.NumberFormat(state.locale === 'vi' ? 'vi-VN' : 'en-US', {
    style: 'currency',
    currency: 'VND',
    maximumFractionDigits: 0
  }).format(Number(value || 0));
}

function formatDate(value) {
  return new Intl.DateTimeFormat(state.locale === 'vi' ? 'vi-VN' : 'en-US', {
    dateStyle: 'short',
    timeStyle: 'short'
  }).format(new Date(value));
}

function escapeHtml(value) {
  return String(value ?? '')
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
    .replaceAll("'", '&#039;');
}

function escapeAttr(value) {
  return escapeHtml(value);
}

function showToast(message) {
  const toast = document.getElementById('toast');
  toast.textContent = message;
  toast.classList.add('is-visible');
  window.clearTimeout(showToast.timer);
  showToast.timer = window.setTimeout(() => toast.classList.remove('is-visible'), 2400);
}
