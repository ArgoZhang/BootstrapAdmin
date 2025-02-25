import { execute } from "../../_content/BootstrapBlazor/modules/ajax.js"

export async function login(url, userName, rememberMe) {
    var password = document.querySelector('input[type="password"]').value;
    await execute({
        url: url,
        method: 'POST',
        toJson: false,
        data: {
            userName,
            password,
            rememberMe
        }
    });
    location.href = "/";
}