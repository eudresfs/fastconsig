import nodemailer from 'nodemailer'
import type { Transporter } from 'nodemailer'
import { env } from '@/config/env'

/**
 * Email service configuration
 */
interface EmailConfig {
  host: string
  port: number
  secure: boolean
  auth?: {
    user: string
    pass: string
  }
  from: string
}

/**
 * Email options
 */
export interface EmailOptions {
  to: string
  subject: string
  text?: string
  html?: string
}

/**
 * Email service for sending emails
 */
class EmailService {
  private transporter: Transporter | null = null
  private config: EmailConfig

  constructor() {
    this.config = {
      host: env.SMTP_HOST,
      port: env.SMTP_PORT,
      secure: env.SMTP_SECURE,
      auth:
        env.SMTP_USER && env.SMTP_PASSWORD
          ? {
              user: env.SMTP_USER,
              pass: env.SMTP_PASSWORD,
            }
          : undefined,
      from: env.EMAIL_FROM,
    }

    this.initializeTransporter()
  }

  /**
   * Initialize nodemailer transporter
   */
  private initializeTransporter(): void {
    try {
      this.transporter = nodemailer.createTransport({
        host: this.config.host,
        port: this.config.port,
        secure: this.config.secure,
        auth: this.config.auth,
      })
    } catch (error) {
      console.error('Failed to initialize email transporter:', error)
      this.transporter = null
    }
  }

  /**
   * Send email
   */
  async sendEmail(options: EmailOptions): Promise<void> {
    if (!this.transporter) {
      console.warn('Email transporter not configured. Email not sent:', options.subject)
      return
    }

    try {
      await this.transporter.sendMail({
        from: this.config.from,
        to: options.to,
        subject: options.subject,
        text: options.text,
        html: options.html,
      })
    } catch (error) {
      console.error('Failed to send email:', error)
      throw new Error('Falha ao enviar email')
    }
  }

  /**
   * Send password reset email
   */
  async sendPasswordResetEmail(
    email: string,
    resetToken: string,
    appUrl: string
  ): Promise<void> {
    const resetUrl = `${appUrl}/reset-password?token=${resetToken}`

    const html = `
      <!DOCTYPE html>
      <html>
        <head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width, initial-scale=1.0">
          <style>
            body {
              font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
              line-height: 1.6;
              color: #333;
              max-width: 600px;
              margin: 0 auto;
              padding: 20px;
            }
            .container {
              background-color: #f9f9f9;
              border-radius: 8px;
              padding: 30px;
              border: 1px solid #e0e0e0;
            }
            .header {
              text-align: center;
              margin-bottom: 30px;
            }
            .header h1 {
              color: #2563eb;
              margin: 0;
              font-size: 24px;
            }
            .content {
              background-color: white;
              padding: 25px;
              border-radius: 6px;
              margin-bottom: 20px;
            }
            .button {
              display: inline-block;
              padding: 12px 30px;
              background-color: #2563eb;
              color: white !important;
              text-decoration: none;
              border-radius: 6px;
              margin: 20px 0;
              font-weight: 500;
            }
            .button:hover {
              background-color: #1d4ed8;
            }
            .footer {
              text-align: center;
              color: #666;
              font-size: 14px;
              margin-top: 20px;
            }
            .warning {
              background-color: #fef3c7;
              border-left: 4px solid #f59e0b;
              padding: 12px;
              margin: 20px 0;
              border-radius: 4px;
            }
            .link {
              word-break: break-all;
              color: #2563eb;
              font-size: 14px;
            }
          </style>
        </head>
        <body>
          <div class="container">
            <div class="header">
              <h1>FastConsig</h1>
            </div>

            <div class="content">
              <h2>Recuperação de Senha</h2>
              <p>Olá,</p>
              <p>Você solicitou a recuperação de senha da sua conta no FastConsig.</p>
              <p>Clique no botão abaixo para redefinir sua senha:</p>

              <div style="text-align: center;">
                <a href="${resetUrl}" class="button">Redefinir Senha</a>
              </div>

              <p>Ou copie e cole o seguinte link no seu navegador:</p>
              <p class="link">${resetUrl}</p>

              <div class="warning">
                <strong>⚠️ Importante:</strong> Este link expira em 1 hora. Se você não solicitou esta recuperação, ignore este email.
              </div>
            </div>

            <div class="footer">
              <p>Este é um email automático. Por favor, não responda.</p>
              <p>&copy; ${new Date().getFullYear()} FastConsig. Todos os direitos reservados.</p>
            </div>
          </div>
        </body>
      </html>
    `

    const text = `
Recuperação de Senha - FastConsig

Olá,

Você solicitou a recuperação de senha da sua conta no FastConsig.

Para redefinir sua senha, acesse o seguinte link:
${resetUrl}

Este link expira em 1 hora.

Se você não solicitou esta recuperação, ignore este email.

---
Este é um email automático. Por favor, não responda.
© ${new Date().getFullYear()} FastConsig. Todos os direitos reservados.
    `

    await this.sendEmail({
      to: email,
      subject: 'Recuperação de Senha - FastConsig',
      text: text.trim(),
      html,
    })
  }

  /**
   * Send password changed confirmation email
   */
  async sendPasswordChangedEmail(email: string, userName: string): Promise<void> {
    const html = `
      <!DOCTYPE html>
      <html>
        <head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width, initial-scale=1.0">
          <style>
            body {
              font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
              line-height: 1.6;
              color: #333;
              max-width: 600px;
              margin: 0 auto;
              padding: 20px;
            }
            .container {
              background-color: #f9f9f9;
              border-radius: 8px;
              padding: 30px;
              border: 1px solid #e0e0e0;
            }
            .header {
              text-align: center;
              margin-bottom: 30px;
            }
            .header h1 {
              color: #2563eb;
              margin: 0;
              font-size: 24px;
            }
            .content {
              background-color: white;
              padding: 25px;
              border-radius: 6px;
              margin-bottom: 20px;
            }
            .success {
              background-color: #d1fae5;
              border-left: 4px solid #10b981;
              padding: 12px;
              margin: 20px 0;
              border-radius: 4px;
            }
            .warning {
              background-color: #fef3c7;
              border-left: 4px solid #f59e0b;
              padding: 12px;
              margin: 20px 0;
              border-radius: 4px;
            }
            .footer {
              text-align: center;
              color: #666;
              font-size: 14px;
              margin-top: 20px;
            }
          </style>
        </head>
        <body>
          <div class="container">
            <div class="header">
              <h1>FastConsig</h1>
            </div>

            <div class="content">
              <h2>Senha Alterada com Sucesso</h2>
              <p>Olá${userName ? ` ${userName}` : ''},</p>

              <div class="success">
                <strong>✓ Confirmação:</strong> Sua senha foi alterada com sucesso.
              </div>

              <p>Esta é uma confirmação de que a senha da sua conta no FastConsig foi alterada em ${new Date().toLocaleString('pt-BR')}.</p>

              <div class="warning">
                <strong>⚠️ Você não reconhece esta alteração?</strong> Entre em contato com o suporte imediatamente.
              </div>
            </div>

            <div class="footer">
              <p>Este é um email automático. Por favor, não responda.</p>
              <p>&copy; ${new Date().getFullYear()} FastConsig. Todos os direitos reservados.</p>
            </div>
          </div>
        </body>
      </html>
    `

    const text = `
Senha Alterada com Sucesso - FastConsig

Olá${userName ? ` ${userName}` : ''},

Esta é uma confirmação de que a senha da sua conta no FastConsig foi alterada em ${new Date().toLocaleString('pt-BR')}.

Se você não reconhece esta alteração, entre em contato com o suporte imediatamente.

---
Este é um email automático. Por favor, não responda.
© ${new Date().getFullYear()} FastConsig. Todos os direitos reservados.
    `

    await this.sendEmail({
      to: email,
      subject: 'Senha Alterada - FastConsig',
      text: text.trim(),
      html,
    })
  }

  /**
   * Verify email transporter connection
   */
  async verifyConnection(): Promise<boolean> {
    if (!this.transporter) {
      return false
    }

    try {
      await this.transporter.verify()
      return true
    } catch (error) {
      console.error('Email connection verification failed:', error)
      return false
    }
  }
}

// Export singleton instance
export const emailService = new EmailService()
