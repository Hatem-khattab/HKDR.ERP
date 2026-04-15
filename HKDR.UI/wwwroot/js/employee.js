function logout() {
    const refreshToken = localStorage.getItem('refreshToken');
    const token = localStorage.getItem('accessToken');
    fetch('https://localhost:7108/api/auth/logout', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ refreshToken })
    });
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    window.location.href = '../../../index.html';
}
