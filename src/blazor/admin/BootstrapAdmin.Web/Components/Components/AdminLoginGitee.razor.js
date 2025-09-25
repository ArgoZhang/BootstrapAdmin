import Data from "../../_content/BootstrapBlazor/modules/data.js"
import EventHandler from "../../_content/BootstrapBlazor/modules/event-handler.js"

export function init(id, invoke) {
    const el = document.getElementById(id);
    const login = el.querySelector('.btn-login');
    const userName = el.querySelector('input[name="userName"]');
    const password = el.querySelector('input[name="password"]');

    Data.set(id, { el, login, userName, password });

    EventHandler.on(login, 'click', async e => {
        e.preventDefault();

        let valid = validateUserName(userName) && validatePassword(password);
        if (!valid) {
            return;
        }

        valid = await invoke.invokeMethodAsync('TriggerLogin');
        if (!valid) {
            return;
        }

        el.submit();
    });
}

function validateUserName(userName) {
    let ret = true;
    if (userName.value === '') {
        const tooltip = bootstrap.Tooltip.getOrCreateInstance(userName, {
            title: '请输入用户名',
            customClass: 'is-invalid'
        });
        tooltip.show();
        userName.tooltip = tooltip;
        ret = false;
    }
    else if (userName.tooltip) {
        userName.tooltip.dispose();
    }

    return ret;
}

function validatePassword(password) {
    let ret = true;
    if (password.value === '') {
        const tooltip = bootstrap.Tooltip.getOrCreateInstance(password, {
            title: '请输入密码',
            customClass: 'is-invalid'
        });
        tooltip.show();
        password.tooltip = tooltip;
        ret = false;
    }
    else if (password.tooltip) {
        password.tooltip.dispose();
    }

    return ret;
}

export function dispose(id) {
    const data = Data.get(id);
    Data.remove(id);

    const { login, userName, password } = data;

    EventHandler.off(login, 'click');
    if (userName.tooltip != null) {
        userName.tooltip.dispose();
    }
    if (password.tooltip != null) {
        password.tooltip.dispose();
    }
}