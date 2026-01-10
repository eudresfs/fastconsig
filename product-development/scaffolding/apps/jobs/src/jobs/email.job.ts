import { type Job } from 'bullmq'
import nodemailer from 'nodemailer'
import { logger } from '../config/logger'

interface EmailJobData {
  to: string
  subject: string
  html: string
  from?: string
  replyTo?: string
  attachments?: Array<{
    filename: string
    content: Buffer | string
    contentType?: string
  }>
}

const transporter = nodemailer.createTransport({
  host: process.env.SMTP_HOST ?? 'localhost',
  port: Number(process.env.SMTP_PORT) || 1025,
  secure: process.env.SMTP_SECURE === 'true',
  auth: process.env.SMTP_USER
    ? {
        user: process.env.SMTP_USER,
        pass: process.env.SMTP_PASSWORD,
      }
    : undefined,
})

export async function processEmail(job: Job<EmailJobData>): Promise<void> {
  const { to, subject, html, from, replyTo, attachments } = job.data

  logger.info({ to, subject }, 'Sending email')

  try {
    await transporter.sendMail({
      from: from ?? process.env.EMAIL_FROM ?? 'noreply@fastconsig.com.br',
      to,
      subject,
      html,
      replyTo,
      attachments,
    })

    logger.info({ to, subject }, 'Email sent successfully')
  } catch (error) {
    logger.error({ to, subject, error }, 'Failed to send email')
    throw error
  }
}
