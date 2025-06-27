CREATE 
EVENT modular_kitchen_designer.remove_outdated_kitchens_event
	ON SCHEDULE EVERY '1' HOUR
	STARTS '2025-06-27 08:10:00'
	DO 
BEGIN
  DECLARE EXIT HANDLER FOR SQLEXCEPTION
  BEGIN
      GET DIAGNOSTICS CONDITION 1 
      @sqlstate = RETURNED_SQLSTATE, 
      @errno = MYSQL_ERRNO, 
      @text = MESSAGE_TEXT;
      
        -- Пытаемся записать ошибку
      BEGIN
          DECLARE CONTINUE HANDLER FOR SQLEXCEPTION BEGIN END;  -- Пустой обработчик для вложенной ошибки
          
          INSERT INTO error_logs(event_name, error_code, error_message) 
          VALUES ('remove_outdated_kitchens_event', @errno, @text);
      END;
  END;

  DROP TEMPORARY TABLE IF EXISTS temp_kitchens_to_delete;

  SET @current_date = NOW() - INTERVAL 7 DAY;

  CREATE TEMPORARY TABLE IF NOT EXISTS temp_kitchens_to_delete (
      ID CHAR(36) PRIMARY KEY
  ) ENGINE = MEMORY;
  
  -- Наполняем временную таблицу (убрана избыточная проверка IF NOT EXISTS)
  INSERT INTO temp_kitchens_to_delete
  SELECT ID FROM kitchens
  WHERE CreatedAt < @current_date;

  -- Удаляем связанные данные через JOIN (эффективнее чем IN+подзапрос)
  DELETE m FROM material_specification_items m
  INNER JOIN temp_kitchens_to_delete t ON m.KitchenId = t.ID;
  
  DELETE s FROM sections s
  INNER JOIN temp_kitchens_to_delete t ON s.KitchenId = t.ID; 

  -- Удаляем основные записи
  DELETE k FROM kitchens k
  INNER JOIN temp_kitchens_to_delete t ON k.ID = t.ID;

  DROP TEMPORARY TABLE IF EXISTS temp_kitchens_to_delete;

END;

ALTER 
EVENT modular_kitchen_designer.remove_outdated_kitchens_event
	ENABLE