-- Function: public.update_folder_name(integer, character)

-- DROP FUNCTION public.update_folder_name(integer, character);

CREATE OR REPLACE FUNCTION public.update_folder_name(
    integer,
    character)
  RETURNS void AS
$BODY$
BEGIN
	update folder
	set name = $2
	where id = $1;
END;
 
 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION public.update_folder_name(integer, character)
  OWNER TO postgres;
