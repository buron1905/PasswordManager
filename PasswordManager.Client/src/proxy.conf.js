const PROXY_CONFIG = [
  {
    context: [
      "/api/**",
    ],
    target: "https://localhost:5001",
    changeOrigin: true,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
