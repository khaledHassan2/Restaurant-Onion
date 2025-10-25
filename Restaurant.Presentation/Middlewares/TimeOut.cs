namespace Restaurant.Presentation.Middlewares
{
    public class TimeOut
    {
        private readonly RequestDelegate _next;

        public TimeOut(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var currentHour = DateTime.Now.Hour;

            // Block access between 8 PM (20:00) and 8 AM (08:00)
            if (currentHour >= 20 || currentHour < 8)
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "text/html";

                await context.Response.WriteAsync(@"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='utf-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Restaurant Closed</title>
                        <link href='https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.2/css/bootstrap.min.css' rel='stylesheet'>
                        <style>
                            * {
                                margin: 0;
                                padding: 0;
                                box-sizing: border-box;
                            }

                            body {
                                background: linear-gradient(135deg, #2c3e50 0%, #34495e 100%);
                                min-height: 100vh;
                                display: flex;
                                align-items: center;
                                justify-content: center;
                                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                overflow-x: hidden;
                            }

                            .restriction-card {
                                background: white;
                                border-radius: 16px;
                                padding: 3rem 2rem;
                                box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
                                text-align: center;
                                max-width: 550px;
                                animation: slideDown 0.6s ease;
                                position: relative;
                                overflow: hidden;
                            }

                            .restriction-card::before {
                                content: '';
                                position: absolute;
                                top: 0;
                                left: 0;
                                right: 0;
                                height: 4px;
                                background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
                            }

                            @keyframes slideDown {
                                from {
                                    opacity: 0;
                                    transform: translateY(-30px);
                                }
                                to {
                                    opacity: 1;
                                    transform: translateY(0);
                                }
                            }

                            .icon-wrapper {
                                font-size: 80px;
                                color: #e74c3c;
                                margin-bottom: 1.5rem;
                                animation: pulse 2s infinite;
                            }

                            @keyframes pulse {
                                0%, 100% {
                                    transform: scale(1);
                                }
                                50% {
                                    transform: scale(1.15);
                                }
                            }

                            h1 {
                                color: #2c3e50;
                                font-weight: 700;
                                font-size: 2rem;
                                margin-bottom: 0.5rem;
                            }

                            .subtitle {
                                color: #7f8c8d;
                                font-size: 1rem;
                                margin-bottom: 2rem;
                            }

                            /* Status Badge */
                            .status-badge {
                                background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
                                color: white;
                                padding: 0.75rem 1.5rem;
                                border-radius: 8px;
                                font-weight: 600;
                                display: inline-block;
                                margin-bottom: 2rem;
                                font-size: 0.9rem;
                            }

                            /* Timer Section */
                            .timer-section {
                                background: linear-gradient(135deg, #f5f7fa 0%, #e8ecf1 100%);
                                border-radius: 12px;
                                padding: 2rem 1.5rem;
                                margin: 2rem 0;
                                border-left: 4px solid #e74c3c;
                            }

                            .timer-label {
                                color: #7f8c8d;
                                font-size: 0.9rem;
                                font-weight: 600;
                                margin-bottom: 1rem;
                                text-transform: uppercase;
                                letter-spacing: 1px;
                            }

                            .timer-display {
                                font-size: 3rem;
                                font-weight: 700;
                                color: #e74c3c;
                                font-family: 'Courier New', monospace;
                                letter-spacing: 2px;
                                margin: 1rem 0;
                            }

                            .timer-info {
                                color: #2c3e50;
                                font-size: 0.95rem;
                                font-weight: 500;
                            }

                            /* Info Sections */
                            .info-box {
                                background: white;
                                border: 2px solid #ecf0f1;
                                border-radius: 10px;
                                padding: 1.5rem;
                                margin: 1.5rem 0;
                                text-align: left;
                            }

                            .info-box-title {
                                color: #2c3e50;
                                font-weight: 600;
                                font-size: 1rem;
                                margin-bottom: 1rem;
                                display: flex;
                                align-items: center;
                                gap: 0.5rem;
                            }

                            .info-box-title::before {
                                content: '';
                                width: 4px;
                                height: 20px;
                                background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
                                border-radius: 2px;
                            }

                            .hours-grid {
                                display: grid;
                                grid-template-columns: 1fr 1fr;
                                gap: 1rem;
                            }

                            .hours-item {
                                background: linear-gradient(135deg, #f5f7fa 0%, #e8ecf1 100%);
                                padding: 1rem;
                                border-radius: 8px;
                                text-align: center;
                            }

                            .hours-item.closed {
                                background: linear-gradient(135deg, #fadbd8 0%, #f5b7b1 100%);
                            }

                            .hours-item.open {
                                background: linear-gradient(135deg, #d5f4e6 0%, #c8f0e0 100%);
                            }

                            .hours-status {
                                font-weight: 600;
                                font-size: 0.85rem;
                                margin-bottom: 0.5rem;
                            }

                            .hours-closed {
                                color: #e74c3c;
                            }

                            .hours-open {
                                color: #27ae60;
                            }

                            .hours-time {
                                color: #2c3e50;
                                font-weight: 700;
                                font-size: 1.1rem;
                            }

                            .current-time {
                                background: linear-gradient(135deg, #3498db 0%, #2980b9 100%);
                                color: white;
                                padding: 1rem;
                                border-radius: 8px;
                                margin-top: 1rem;
                                font-weight: 600;
                            }

                            /* Message */
                            .message-box {
                                background: linear-gradient(135deg, #fef5f5 0%, #fff9f0 100%);
                                border-left: 4px solid #f39c12;
                                border-radius: 8px;
                                padding: 1rem;
                                margin: 1.5rem 0;
                                color: #2c3e50;
                                font-size: 0.95rem;
                            }

                            /* Button */
                            .btn-home {
                                background: linear-gradient(135deg, #27ae60 0%, #229954 100%);
                                color: white;
                                padding: 0.85rem 2rem;
                                border-radius: 8px;
                                border: none;
                                font-weight: 600;
                                text-decoration: none;
                                display: inline-block;
                                margin-top: 1.5rem;
                                transition: all 0.3s ease;
                                cursor: pointer;
                                font-size: 1rem;
                            }

                            .btn-home:hover {
                                transform: translateY(-2px);
                                box-shadow: 0 10px 25px rgba(39, 174, 96, 0.4);
                                color: white;
                            }

                            /* Responsive */
                            @media (max-width: 600px) {
                                .restriction-card {
                                    padding: 2rem 1.5rem;
                                    margin: 1rem;
                                }

                                h1 {
                                    font-size: 1.5rem;
                                }

                                .timer-display {
                                    font-size: 2.5rem;
                                }

                                .hours-grid {
                                    grid-template-columns: 1fr;
                                }

                                .info-box {
                                    padding: 1rem;
                                }
                            }
                        </style>
                    </head>
                    <body>
                        <div class='restriction-card'>
                            <div class='icon-wrapper'>⏰</div>
                            
                            <h1>Restaurant Closed</h1>
                            <p class='subtitle'>We're currently closed for the night</p>
                            
                            <div class='status-badge'>
                                🌙 Operating Hours: 8:00 AM - 8:00 PM
                            </div>

                            <!-- Timer Section -->
                            <div class='timer-section'>
                                <div class='timer-label'>⏳ Time Until We Open</div>
                                <div class='timer-display' id='timer'>--:--:--</div>
                                <div class='timer-info'>Come back when we reopen!</div>
                            </div>

                            <!-- Operating Hours -->
                            <div class='info-box'>
                                <div class='info-box-title'>Operating Hours</div>
                                <div class='hours-grid'>
                                    <div class='hours-item closed'>
                                        <div class='hours-status hours-closed'>🌙 Closed</div>
                                        <div class='hours-time'>8:00 PM - 8:00 AM</div>
                                    </div>
                                    <div class='hours-item open'>
                                        <div class='hours-status hours-open'>☀️ Open</div>
                                        <div class='hours-time'>8:00 AM - 8:00 PM</div>
                                    </div>
                                </div>
                                <div class='current-time'>
                                    🕐 Current Time: " + DateTime.Now.ToString("hh:mm tt") + @"
                                </div>
                            </div>

                            <!-- Message -->
                            <div class='message-box'>
                                <strong>💡 Tip:</strong> Bookmark us and visit during our operating hours to enjoy our delicious menu!
                            </div>
                        </div>

                        <script>
                            function updateTimer() {
                                const now = new Date();
                                let hours = now.getHours();
                                const minutes = now.getMinutes();
                                const seconds = now.getSeconds();

                                let timeUntilOpen;

                                if (hours >= 20) {
                                    // After 8 PM, calculate time until 8 AM next day
                                    timeUntilOpen = (24 - hours + 8) * 3600 + (60 - minutes) * 60 + (60 - seconds);
                                } else if (hours < 8) {
                                    // Before 8 AM, calculate time until 8 AM
                                    timeUntilOpen = (8 - hours) * 3600 + (60 - minutes) * 60 + (60 - seconds);
                                }

                                const h = Math.floor(timeUntilOpen / 3600);
                                const m = Math.floor((timeUntilOpen % 3600) / 60);
                                const s = timeUntilOpen % 60;

                                document.getElementById('timer').textContent = 
                                    String(h).padStart(2, '0') + ':' +
                                    String(m).padStart(2, '0') + ':' +
                                    String(s).padStart(2, '0');
                            }

                            // Update timer immediately and then every second
                            updateTimer();
                            setInterval(updateTimer, 1000);
                        </script>
                    </body>
                    </html>
                ");

                return;
            }

            await _next(context);
        }
    }
}