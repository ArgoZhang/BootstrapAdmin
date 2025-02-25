import { execute } from "../../_content/BootstrapBlazor/modules/ajax.js"

export async function login(url, userName, rememberMe) {
    const password = document.querySelector('input[type="password"]').value;
    const response = await execute({
        url: url,
        method: 'POST',
        toJson: false,
        data: {
            userName,
            password,
            rememberMe
        }
    });
    if (response.redirected) {
        window.location.href = response.url;
    }
}