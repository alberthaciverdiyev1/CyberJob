package utils

import (
	"os"

	"go.uber.org/zap"
	"go.uber.org/zap/zapcore"
	"gopkg.in/natefinch/lumberjack.v2"
)

var Log *zap.Logger

func InitLogger() {

	lumberjackLogger := &lumberjack.Logger{
		Filename:   "logs/cyberjob.log",
		MaxSize:    5, // MB
		MaxBackups: 30,
		MaxAge:     30,
		Compress:   true,
		LocalTime:  true,
	}

	fileWriter := zapcore.AddSync(lumberjackLogger)

	encoderConfig := zap.NewProductionEncoderConfig()
	encoderConfig.EncodeTime = zapcore.ISO8601TimeEncoder

	fileEncoder := zapcore.NewJSONEncoder(encoderConfig)
	consoleEncoder := zapcore.NewConsoleEncoder(zap.NewDevelopmentEncoderConfig())

	core := zapcore.NewTee(
		zapcore.NewCore(consoleEncoder, zapcore.AddSync(os.Stdout), zap.DebugLevel),
		zapcore.NewCore(fileEncoder, fileWriter, zap.InfoLevel),
	)

	Log = zap.New(core, zap.AddCaller())
}
