// Custom JavaScript for Swagger UI enhancements

window.addEventListener('load', function() {
    // Add curl command generation
    function generateCurlCommand(operation, spec) {
        if (!operation || !spec) return '';

        const method = operation.method.toUpperCase();
        const url = window.location.origin + operation.url;
        const headers = operation.parameters
            ? operation.parameters.filter(p => p.in === 'header')
            : [];

        let curl = `curl -X ${method} "${url}"`;

        // Add headers
        headers.forEach(header => {
            curl += ` \\\n  -H "${header.name}: {${header.name}}"`;
        });

        // Add content-type for POST/PUT/PATCH
        if (['POST', 'PUT', 'PATCH'].includes(method)) {
            curl += ` \\\n  -H "Content-Type: application/json"`;
        }

        // Add body for POST/PUT/PATCH
        if (['POST', 'PUT', 'PATCH'].includes(method) && operation.requestBody) {
            curl += ` \\\n  -d '{}'`;
        }

        return curl;
    }

    // Add curl sections to operations
    function addCurlSections() {
        const operations = document.querySelectorAll('.swagger-ui .opblock');

        operations.forEach(operation => {
            const curlSection = operation.querySelector('.curl-section');
            if (curlSection) return; // Already added

            const summary = operation.querySelector('.opblock-summary');
            if (!summary) return;

            const curlDiv = document.createElement('div');
            curlDiv.className = 'curl-section';
            curlDiv.style.cssText = `
                background: #2d3748;
                color: #e2e8f0;
                padding: 12px;
                border-radius: 6px;
                font-family: Monaco, Consolas, monospace;
                font-size: 13px;
                margin: 10px 20px;
                display: none;
                overflow-x: auto;
            `;

            const method = summary.querySelector('.opblock-summary-method')?.textContent || '';
            const path = summary.querySelector('.opblock-summary-path')?.textContent || '';
            const curl = `curl -X ${method.toUpperCase()} "${window.location.origin}${path}" \\
  -H "Content-Type: application/json" \\
  -H "Authorization: Bearer {token}"`;

            curlDiv.innerHTML = `<strong>cURL:</strong><br><code>${curl}</code>`;

            // Add toggle button
            const toggleBtn = document.createElement('button');
            toggleBtn.textContent = 'Show cURL';
            toggleBtn.className = 'btn curl-toggle-btn';
            toggleBtn.style.cssText = `
                background: #17a2b8;
                color: white;
                border: none;
                padding: 5px 10px;
                border-radius: 4px;
                font-size: 12px;
                margin-left: 10px;
                cursor: pointer;
            `;

            toggleBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                if (curlDiv.style.display === 'none') {
                    curlDiv.style.display = 'block';
                    toggleBtn.textContent = 'Hide cURL';
                } else {
                    curlDiv.style.display = 'none';
                    toggleBtn.textContent = 'Show cURL';
                }
            });

            summary.appendChild(toggleBtn);
            operation.appendChild(curlDiv);
        });
    }

    // Initialize curl sections
    setTimeout(addCurlSections, 1000);

    // Re-add curl sections when operations are expanded/collapsed
    document.addEventListener('click', function(e) {
        if (e.target.closest('.opblock-summary')) {
            setTimeout(addCurlSections, 500);
        }
    });

    // Add a professional footer
    const footer = document.createElement('div');
    footer.style.cssText = `
        text-align: center;
        padding: 20px;
        color: #6c757d;
        font-size: 14px;
        border-top: 1px solid #e9ecef;
        margin-top: 40px;
    `;
    footer.innerHTML = 'API Documentation - Read Only Mode';

    const container = document.querySelector('.swagger-ui .swagger-container');
    if (container) {
        container.appendChild(footer);
    }
});